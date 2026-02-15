using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetPainPerRecentMemory(CardboxDbContext context) : IGetPainPerRecentMemory
{
    public async Task<IEnumerable<PainStats>> ExecuteAsync(int limit = 100)
    {
        string sql = $"""
            SELECT
              question,
              incorrect,
              correct,
              streak,
              cardbox,
              difficulty
            FROM questions
            WHERE cardbox IS NOT NULL
              AND incorrect >= 8
              AND streak <= 2
            ORDER BY incorrect DESC
            LIMIT {limit};
            """;

        return await context.Database.SqlQueryRaw<PainStats>(sql).ToListAsync();
    }
}
