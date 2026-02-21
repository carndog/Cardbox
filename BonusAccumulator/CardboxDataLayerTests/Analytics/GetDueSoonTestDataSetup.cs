using CardboxDataLayer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CardboxDataLayerTests.Analytics;

public static class GetDueSoonTestDataSetup
{
    private const string TestDatabasePath = "getduesoon_test_anagrams.db";

    public static CardboxDbContext CreateTestContext()
    {
        string connectionString = $"Data Source={TestDatabasePath}";
        DbContextOptionsBuilder<CardboxDbContext> optionsBuilder = new DbContextOptionsBuilder<CardboxDbContext>()
            .UseSqlite(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        DbContextOptions<CardboxDbContext> options = optionsBuilder.Options;

        CardboxDbContext context = new CardboxDbContext(options);
        
        EnsureDatabaseCreated(context);
        SeedDueSoonTestData(context);
        
        return context;
    }

    public static void CleanupTestDatabase()
    {
        try
        {
            if (File.Exists(TestDatabasePath))
            {
                File.Delete(TestDatabasePath);
            }
        }
        catch (IOException)
        {
        }
    }

    private static void EnsureDatabaseCreated(CardboxDbContext context)
    {
        context.Database.EnsureCreated();
    }

    private static void SeedDueSoonTestData(CardboxDbContext context)
    {
        if (context.Questions.Any())
        {
            return;
        }

        using DbConnection connection = context.Database.GetDbConnection();
        connection.Open();

        using DbTransaction transaction = connection.BeginTransaction();

        try
        {
            int fixedTestTime = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string insertQuestionsSql = $@"
                INSERT INTO questions (question, correct, incorrect, streak, last_correct, difficulty, cardbox, next_scheduled)
                VALUES 
                    ('DUESOON1', 6, 2, 3, {fixedTestTime - 86400}, 2, 1, {fixedTestTime + 3600}),
                    ('DUESOON2', 9, 1, 4, {fixedTestTime - 172800}, 3, 2, {fixedTestTime + 7200}),
                    ('DUESOON3', 4, 3, 2, {fixedTestTime - 86400}, 1, 1, {fixedTestTime + 43200}),
                    
                    ('OVERDUE1', 5, 3, 2, {fixedTestTime - 172800}, 2, 1, {fixedTestTime - 3600}),
                    ('OVERDUE2', 8, 4, 1, {fixedTestTime - 86400}, 3, 2, {fixedTestTime - 7200}),
                    
                    ('FUTURE1', 10, 1, 5, {fixedTestTime - 86400}, 4, 3, {fixedTestTime + 100800}),
                    ('FUTURE2', 15, 2, 6, {fixedTestTime - 172800}, 5, 4, {fixedTestTime + 172800})";

            using DbCommand command = connection.CreateCommand();
            command.CommandText = insertQuestionsSql;
            command.Transaction = transaction;
            command.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
