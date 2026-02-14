using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetPainPerRecentMemory : IGetPainPerRecentMemory
{
    private readonly CardboxDbContext _context;

    public GetPainPerRecentMemory(CardboxDbContext context)
    {
        _context = context;
    }

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
            WHERE incorrect >= 8
              AND streak <= 2
            ORDER BY incorrect DESC
            LIMIT {limit};
            """;

        return await _context.Database.SqlQueryRaw<PainStats>(sql).ToListAsync();
    }
}
