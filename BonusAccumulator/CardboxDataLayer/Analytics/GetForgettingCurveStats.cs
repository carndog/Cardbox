using Microsoft.EntityFrameworkCore;
using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class GetForgettingCurveStats : IGetForgettingCurveStats
{
    private readonly CardboxDbContext _context;

    public GetForgettingCurveStats(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ForgettingCurveStats>> ExecuteAsync()
    {
        const string sql = """
            WITH buckets AS (
              SELECT
                CASE
                  WHEN (strftime('%s','now') - last_correct) <  1*86400 THEN '0-1d'
                  WHEN (strftime('%s','now') - last_correct) <  3*86400 THEN '1-3d'
                  WHEN (strftime('%s','now') - last_correct) <  7*86400 THEN '3-7d'
                  WHEN (strftime('%s','now') - last_correct) < 14*86400 THEN '7-14d'
                  WHEN (strftime('%s','now') - last_correct) < 30*86400 THEN '14-30d'
                  ELSE '30d+'
                END AS AgeBucket,
                correct,
                incorrect
              FROM questions
              WHERE (correct + incorrect) >= 5
                AND last_correct > 0
            )
            SELECT
              AgeBucket,
              COUNT(*) AS Items,
              ROUND(100.0 * SUM(correct) / NULLIF(SUM(correct + incorrect), 0), 1) AS PctCorrect
            FROM buckets
            GROUP BY AgeBucket
            ORDER BY
              CASE AgeBucket
                WHEN '0-1d' THEN 1
                WHEN '1-3d' THEN 2
                WHEN '3-7d' THEN 3
                WHEN '7-14d' THEN 4
                WHEN '14-30d' THEN 5
                ELSE 6
              END;
            """;

        return await _context.Database.SqlQueryRaw<ForgettingCurveStats>(sql).ToListAsync();
    }
}
