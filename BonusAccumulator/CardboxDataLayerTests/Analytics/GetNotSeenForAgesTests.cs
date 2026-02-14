using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetNotSeenForAgesTests
{
    private CardboxDbContext _context;
    private GetNotSeenForAges _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetNotSeenForAges(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsNotSeenForLongTime()
    {
        IEnumerable<NotSeenForAgesStats> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        NotSeenForAgesStats firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.LastCorrectAt, Is.LessThan(DateTime.UtcNow));
        Assert.That(firstItem.DaysSinceLastCorrect, Is.GreaterThan(0.0));
    }
}
