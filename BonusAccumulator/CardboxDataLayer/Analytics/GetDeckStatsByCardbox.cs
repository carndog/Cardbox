using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetDeckStatsByCardbox : IGetDeckStatsByCardbox
{
    private readonly CardboxDbContext _context;

    public GetDeckStatsByCardbox(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardboxStats>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              cardbox,
              COUNT(*)                       AS Items,
              SUM(correct + incorrect)       AS TotalReviews,
              ROUND(1.0 * SUM(correct) / NULLIF(SUM(correct + incorrect), 0), 3) AS PercentCorrect
            FROM questions
            WHERE cardbox IS NOT NULL
            GROUP BY cardbox
            ORDER BY cardbox;
            """;

        return await _context.Database.SqlQueryRaw<CardboxStats>(sql).ToListAsync();
    }
}
