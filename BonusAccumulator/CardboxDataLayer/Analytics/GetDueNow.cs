using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetDueNow(CardboxDbContext context) : IGetDueNow
{
    public async Task<IEnumerable<DueItem>> ExecuteAsync(int limit = 200)
    {
        string sql = $"""
            SELECT
              question,
              cardbox,
              difficulty,
              datetime(next_scheduled, 'unixepoch') AS DueAt
            FROM questions
            WHERE cardbox IS NOT NULL AND next_scheduled <= strftime('%s','now')
            ORDER BY next_scheduled ASC
            LIMIT {limit};
            """;

        return await context.Database.SqlQueryRaw<DueItem>(sql).ToListAsync();
    }
}
