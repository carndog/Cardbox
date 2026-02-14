using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetBlindSpots : IGetBlindSpots
{
    private readonly CardboxDbContext _context;

    public GetBlindSpots(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlindSpotStats>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              difficulty,
              LENGTH(question) AS Length,
              COUNT(*) AS Items,
              ROUND(1.0 * SUM(correct) / NULLIF(SUM(correct + incorrect), 0), 3) AS PercentCorrect,
              SUM(correct + incorrect) AS Reviews
            FROM questions
            WHERE cardbox IS NOT NULL
            GROUP BY difficulty, Length
            HAVING Reviews >= 30
            ORDER BY PercentCorrect ASC, Reviews DESC;
            """;

        return await _context.Database.SqlQueryRaw<BlindSpotStats>(sql).ToListAsync();
    }
}
