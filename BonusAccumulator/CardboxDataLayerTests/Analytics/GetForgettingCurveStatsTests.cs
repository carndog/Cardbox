using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetForgettingCurveStatsTests
{
    private CardboxDbContext _context = null!;
    private GetForgettingCurveStats _query = null!;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetForgettingCurveStats(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnAccuracyByTimeBuckets()
    {
        IEnumerable<ForgettingCurveStats> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        ForgettingCurveStats firstBucket = result.First();
        Assert.That(firstBucket.AgeBucket, Is.Not.Null.And.Not.Empty);
        Assert.That(firstBucket.Items, Is.GreaterThan(0));
        Assert.That(firstBucket.PercentCorrect, Is.GreaterThanOrEqualTo(0.0));
    }
}
