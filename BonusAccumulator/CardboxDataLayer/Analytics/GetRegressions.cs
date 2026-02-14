using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetRegressions : IGetRegressions
{
    private readonly CardboxDbContext _context;

    public GetRegressions(CardboxDbContext context)
    {
        _context = context;
    }

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
            WHERE correct >= 20
              AND streak <= 2
            ORDER BY (incorrect * 1.0 / NULLIF(correct, 0)) DESC, incorrect DESC
            LIMIT {limit};
            """;

        return await _context.Database.SqlQueryRaw<RegressionStats>(sql).ToListAsync();
    }
}
