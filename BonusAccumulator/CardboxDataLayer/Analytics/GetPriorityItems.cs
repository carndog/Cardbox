using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetPriorityItems : IGetPriorityItems
{
    private readonly CardboxDbContext _context;

    public GetPriorityItems(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PriorityItem>> ExecuteAsync(int limit = 200)
    {
        string sql = $"""
            WITH base AS (
              SELECT
                question,
                correct,
                incorrect,
                streak,
                cardbox,
                difficulty,
                next_scheduled,
                (correct + incorrect) AS Attempts,
                (next_scheduled <= strftime('%s','now')) AS IsDue,
                (1.0 * incorrect / NULLIF(correct + incorrect, 0)) AS ErrorRate
              FROM questions
              WHERE cardbox IS NOT NULL AND (correct + incorrect) >= 5
            )
            SELECT
              question,
              cardbox,
              difficulty,
              correct,
              incorrect,
              streak,
              datetime(next_scheduled, 'unixepoch') AS DueAt,
              ROUND(
                  (CASE WHEN IsDue THEN 2.0 ELSE 0.5 END)
                * (1.0 + ErrorRate)
                * (1.0 + (3 - MIN(streak, 3)) * 0.25)
              , 3) AS Priority
            FROM base
            ORDER BY Priority DESC, incorrect DESC
            LIMIT {limit};
            """;

        return await _context.Database.SqlQueryRaw<PriorityItem>(sql).ToListAsync();
    }
}
