using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetHighestErrorRateTests
{
    private CardboxDbContext _context = null!;
    private GetHighestErrorRate _query = null!;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetHighestErrorRate(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsWithHighErrorRates()
    {
        IEnumerable<ErrorRateStats> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        ErrorRateStats firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Correct, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Incorrect, Is.GreaterThan(0));
        Assert.That(firstItem.Attempts, Is.GreaterThanOrEqualTo(10));
        Assert.That(firstItem.ErrorRate, Is.GreaterThan(0.0));
        Assert.That(firstItem.Streak, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
    }
}
