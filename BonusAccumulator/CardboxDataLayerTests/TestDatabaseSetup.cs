using CardboxDataLayer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CardboxDataLayerTests;

public static class TestDatabaseSetup
{
    private const string TestDatabasePath = "test_anagrams.db";

    public static CardboxDbContext CreateTestContext()
    {
        string connectionString = $"Data Source={TestDatabasePath}";
        DbContextOptionsBuilder<CardboxDbContext> optionsBuilder = new DbContextOptionsBuilder<CardboxDbContext>()
            .UseSqlite(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        DbContextOptions<CardboxDbContext> options = optionsBuilder.Options;

        CardboxDbContext context = new CardboxDbContext(options);
        
        EnsureDatabaseCreated(context);
        SeedTestData(context);
        
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

    private static void SeedTestData(CardboxDbContext context)
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
            // Insert test questions
            string insertQuestionsSql = @"
                INSERT INTO questions (question, correct, incorrect, streak, last_correct, difficulty, cardbox, next_scheduled)
                VALUES 
                    ('HELLO', 5, 2, 3, 12345, 2, 1, 12346),
                    ('WORLD', 10, 1, 5, 12347, 3, 2, 12348),
                    ('TEST', 3, 4, 0, NULL, 1, 1, NULL),
                    ('DATA', 8, 0, 8, 12349, 4, 3, 12350),
                    ('LAYER', 2, 6, 1, 12351, 5, 4, 12352)";

            using DbCommand command = connection.CreateCommand();
            command.CommandText = insertQuestionsSql;
            command.Transaction = transaction;
            command.ExecuteNonQuery();

            // Try to insert test history (may fail if table doesn't exist)
            try
            {
                string insertHistorySql = @"
                    INSERT INTO next_Added (question, timeStamp)
                    VALUES ('HELLO', 12345),
                           ('WORLD', 12346)";

                command.CommandText = insertHistorySql;
                command.ExecuteNonQuery();
            }
            catch
            {
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
