using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetRegressionsTests
{
    private CardboxDbContext _context;
    private GetRegressions _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetRegressions(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsWithManyCorrectButLowStreak()
    {
        IEnumerable<RegressionStats> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        RegressionStats firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Correct, Is.GreaterThanOrEqualTo(20));
        Assert.That(firstItem.Incorrect, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Streak, Is.LessThanOrEqualTo(2));
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.LastCorrectAt, Is.LessThan(DateTime.UtcNow));
    }
}
