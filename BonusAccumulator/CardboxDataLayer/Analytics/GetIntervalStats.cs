using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetIntervalStats(CardboxDbContext context) : IGetIntervalStats
{
    public async Task<IEnumerable<IntervalStats>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              cardbox,
              COUNT(*) AS Items,
              ROUND(AVG((next_scheduled - last_correct) / 86400.0), 2) AS AverageIntervalDays,
              ROUND(MIN((next_scheduled - last_correct) / 86400.0), 2) AS MinimumIntervalDays,
              ROUND(MAX((next_scheduled - last_correct) / 86400.0), 2) AS MaximumIntervalDays
            FROM questions
            WHERE cardbox IS NOT NULL AND last_correct > 0 AND next_scheduled > 0
            GROUP BY cardbox
            ORDER BY cardbox;
            """;

        return await context.Database.SqlQueryRaw<IntervalStats>(sql).ToListAsync();
    }
}
