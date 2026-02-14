using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetIntervalStatsTests
{
    private CardboxDbContext _context;
    private GetIntervalStats _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetIntervalStats(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnIntervalStatisticsByCardbox()
    {
        IEnumerable<IntervalStats> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        IntervalStats firstInterval = result.First();
        Assert.That(firstInterval.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstInterval.Items, Is.GreaterThan(0));
        Assert.That(firstInterval.AverageIntervalDays, Is.GreaterThanOrEqualTo(0.0));
        Assert.That(firstInterval.MinimumIntervalDays, Is.GreaterThanOrEqualTo(0.0));
        Assert.That(firstInterval.MaximumIntervalDays, Is.GreaterThanOrEqualTo(0.0));
    }
}
