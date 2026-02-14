using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetMostWrong : IGetMostWrong
{
    private readonly CardboxDbContext _context;

    public GetMostWrong(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MostWrongStats>> ExecuteAsync(int limit = 100)
    {
        string sql = $"""
            SELECT
              question,
              incorrect,
              correct,
              (correct + incorrect) AS Attempts,
              streak,
              cardbox,
              difficulty
            FROM questions
            WHERE cardbox IS NOT NULL
            ORDER BY incorrect DESC, Attempts DESC
            LIMIT {limit};
            """;

        return await _context.Database.SqlQueryRaw<MostWrongStats>(sql).ToListAsync();
    }
}
