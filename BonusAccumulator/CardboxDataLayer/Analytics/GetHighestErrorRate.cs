using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetHighestErrorRate(CardboxDbContext context) : IGetHighestErrorRate
{
    public async Task<IEnumerable<ErrorRateStats>> ExecuteAsync(int limit = 100)
    {
        string sql = $"""
            SELECT
              question,
              correct,
              incorrect,
              (correct + incorrect) AS Attempts,
              ROUND(1.0 * incorrect / (correct + incorrect), 3) AS ErrorRate,
              streak,
              cardbox,
              difficulty
            FROM questions
            WHERE cardbox IS NOT NULL AND (correct + incorrect) >= 10
            ORDER BY ErrorRate DESC, incorrect DESC
            LIMIT {limit};
            """;

        return await context.Database.SqlQueryRaw<ErrorRateStats>(sql).ToListAsync();
    }
}
