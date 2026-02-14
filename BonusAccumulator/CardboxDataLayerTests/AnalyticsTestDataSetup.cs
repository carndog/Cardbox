using CardboxDataLayer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CardboxDataLayerTests;

public static class AnalyticsTestDataSetup
{
    private const string TestDatabasePath = "analytics_test_anagrams.db";

    public static CardboxDbContext CreateAnalyticsTestContext()
    {
        string connectionString = $"Data Source={TestDatabasePath}";
        DbContextOptionsBuilder<CardboxDbContext> optionsBuilder = new DbContextOptionsBuilder<CardboxDbContext>()
            .UseSqlite(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        DbContextOptions<CardboxDbContext> options = optionsBuilder.Options;

        CardboxDbContext context = new CardboxDbContext(options);
        
        EnsureDatabaseCreated(context);
        SeedAnalyticsTestData(context);
        
        return context;
    }

    public static void CleanupAnalyticsTestDatabase()
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

    private static void SeedAnalyticsTestData(CardboxDbContext context)
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
            int currentTime = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string insertQuestionsSql = $@"
                INSERT INTO questions (question, correct, incorrect, streak, last_correct, difficulty, cardbox, next_scheduled)
                VALUES 
                    -- Health check data - different cardboxes and lengths
                    ('HELLO', 5, 2, 3, {currentTime - 86400}, 2, 1, {currentTime + 86400}),
                    ('WORLD', 10, 1, 5, {currentTime - 172800}, 3, 2, {currentTime + 172800}),
                    ('TEST', 3, 4, 0, {currentTime - 259200}, 1, 1, {currentTime + 86400}),
                    ('DATA', 8, 0, 8, {currentTime - 86400}, 4, 3, {currentTime + 259200}),
                    ('LAYER', 2, 6, 1, {currentTime - 172800}, 5, 4, {currentTime + 86400}),
                    ('ANAGRAM', 15, 3, 4, {currentTime - 86400}, 2, 2, {currentTime + 172800}),
                    ('PUZZLE', 12, 8, 2, {currentTime - 259200}, 3, 3, {currentTime + 86400}),
                    ('SOLVE', 20, 5, 6, {currentTime - 86400}, 4, 4, {currentTime + 345600}),
                    ('QUERY', 7, 9, 1, {currentTime - 172800}, 2, 1, {currentTime + 86400}),
                    ('SEARCH', 25, 2, 8, {currentTime - 86400}, 5, 5, {currentTime + 432000}),
                    
                    -- Due items (overdue and due soon)
                    ('OVERDUE1', 5, 3, 2, {currentTime - 172800}, 2, 1, {currentTime - 3600}),
                    ('OVERDUE2', 8, 4, 1, {currentTime - 86400}, 3, 2, {currentTime - 7200}),
                    ('DUESOON1', 6, 2, 3, {currentTime - 86400}, 2, 1, {currentTime + 3600}),
                    ('DUESOON2', 9, 1, 4, {currentTime - 172800}, 3, 2, {currentTime + 7200}),
                    
                    -- High error rate items (leeched)
                    ('ERRORPRONE1', 5, 15, 0, {currentTime - 86400}, 1, 1, {currentTime + 86400}),
                    ('ERRORPRONE2', 3, 12, 1, {currentTime - 172800}, 2, 2, {currentTime + 86400}),
                    ('MOSTWRONG1', 8, 20, 2, {currentTime - 86400}, 3, 3, {currentTime + 172800}),
                    ('MOSTWRONG2', 4, 18, 0, {currentTime - 259200}, 2, 1, {currentTime + 86400}),
                    ('PAINFUL1', 2, 10, 1, {currentTime - 86400}, 1, 1, {currentTime + 86400}),
                    ('PAINFUL2', 1, 9, 2, {currentTime - 172800}, 2, 2, {currentTime + 86400}),
                    
                    -- Regression items (high correct but low streak)
                    ('REGRESS1', 25, 5, 1, {currentTime - 86400}, 3, 3, {currentTime + 86400}),
                    ('REGRESS2', 30, 8, 2, {currentTime - 172800}, 4, 4, {currentTime + 172800}),
                    ('FORGOTTEN1', 20, 3, 0, {currentTime - 604800}, 2, 2, {currentTime + 86400}),
                    ('FORGOTTEN2', 18, 2, 1, {currentTime - 1209600}, 3, 3, {currentTime + 172800}),
                    
                    -- Different word lengths for blind spot analysis
                    ('CAT', 4, 1, 3, {currentTime - 86400}, 1, 1, {currentTime + 86400}),
                    ('DOG', 5, 2, 2, {currentTime - 172800}, 2, 2, {currentTime + 172800}),
                    ('BIRD', 3, 3, 1, {currentTime - 86400}, 2, 1, {currentTime + 86400}),
                    ('FISH', 6, 1, 4, {currentTime - 259200}, 3, 3, {currentTime + 259200}),
                    ('HOUSE', 8, 2, 3, {currentTime - 86400}, 3, 2, {currentTime + 172800}),
                    ('GARDEN', 7, 3, 2, {currentTime - 172800}, 2, 3, {currentTime + 86400}),
                    ('WINDOW', 9, 1, 5, {currentTime - 86400}, 4, 4, {currentTime + 345600}),
                    ('COMPUTER', 12, 4, 3, {currentTime - 259200}, 4, 3, {currentTime + 259200}),
                    ('KEYBOARD', 10, 5, 2, {currentTime - 86400}, 3, 2, {currentTime + 172800}),
                    ('MONITOR', 11, 3, 4, {currentTime - 172800}, 3, 3, {currentTime + 345600})";

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
