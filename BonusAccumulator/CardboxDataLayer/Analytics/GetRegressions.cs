using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetRegressions(CardboxDbContext context) : IGetRegressions
{
    public async Task<IEnumerable<RegressionStats>> ExecuteAsync(int limit = 100)
    {
        string sql = $"""
            SELECT
              question,
              correct,
              incorrect,
              streak,
              cardbox,
              difficulty,
              datetime(last_correct, 'unixepoch') AS LastCorrectAt
            FROM questions
            WHERE cardbox IS NOT NULL
              AND correct >= 20
              AND streak <= 2
            ORDER BY (incorrect * 1.0 / NULLIF(correct, 0)) DESC, incorrect DESC
            LIMIT {limit};
            """;

        return await context.Database.SqlQueryRaw<RegressionStats>(sql).ToListAsync();
    }
}
