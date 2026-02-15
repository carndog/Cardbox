using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetDeckStatsByWordLength(CardboxDbContext context) : IGetDeckStatsByWordLength
{
    public async Task<IEnumerable<WordLengthStats>> ExecuteAsync()
    {
        const string sql = """
            SELECT
              LENGTH(question) AS Length,
              COUNT(*)         AS Items,
              ROUND(1.0 * SUM(correct) / NULLIF(SUM(correct + incorrect), 0), 3) AS PercentCorrect
            FROM questions
            WHERE cardbox IS NOT NULL
            GROUP BY Length
            ORDER BY Length;
            """;

        return await context.Database.SqlQueryRaw<WordLengthStats>(sql).ToListAsync();
    }
}
