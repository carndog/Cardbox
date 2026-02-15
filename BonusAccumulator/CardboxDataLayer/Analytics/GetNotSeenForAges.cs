using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetNotSeenForAges(CardboxDbContext context) : IGetNotSeenForAges
{
    public async Task<IEnumerable<NotSeenForAgesStats>> ExecuteAsync(int limit = 200)
    {
        string sql = $"""
            SELECT
              question,
              cardbox,
              difficulty,
              datetime(last_correct, 'unixepoch') AS LastCorrectAt,
              ROUND((strftime('%s','now') - last_correct) / 86400.0, 1) AS DaysSinceLastCorrect
            FROM questions
            WHERE cardbox IS NOT NULL AND last_correct > 0
            ORDER BY DaysSinceLastCorrect DESC
            LIMIT {limit};
            """;

        return await context.Database.SqlQueryRaw<NotSeenForAgesStats>(sql).ToListAsync();
    }
}
