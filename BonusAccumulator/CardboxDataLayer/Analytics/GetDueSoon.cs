using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetDueSoon : IGetDueSoon
{
    private readonly CardboxDbContext _context;

    public GetDueSoon(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DueItem>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              question,
              cardbox,
              difficulty,
              datetime(next_scheduled, 'unixepoch') AS DueAt
            FROM questions
            WHERE cardbox IS NOT NULL AND next_scheduled BETWEEN strftime('%s','now') AND strftime('%s','now') + 86400
            ORDER BY next_scheduled ASC;
            """;

        return await _context.Database.SqlQueryRaw<DueItem>(sql).ToListAsync();
    }
}
