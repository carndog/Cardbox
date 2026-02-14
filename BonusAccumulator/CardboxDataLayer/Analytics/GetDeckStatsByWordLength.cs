using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetDeckStatsByWordLength : IGetDeckStatsByWordLength
{
    private readonly CardboxDbContext _context;

    public GetDeckStatsByWordLength(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WordLengthStats>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              LENGTH(question) AS Length,
              COUNT(*)         AS Items,
              ROUND(100.0 * SUM(correct) / NULLIF(SUM(correct + incorrect), 0), 1) AS PctCorrect
            FROM questions
            WHERE cardbox IS NOT NULL
            GROUP BY Length
            ORDER BY Length;
            """;

        return await _context.Database.SqlQueryRaw<WordLengthStats>(sql).ToListAsync();
    }
}
