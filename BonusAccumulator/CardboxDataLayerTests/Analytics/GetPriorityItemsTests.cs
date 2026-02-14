using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetPriorityItemsTests
{
    private CardboxDbContext _context;
    private GetPriorityItems _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetPriorityItems(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsWithPriorityScores()
    {
        IEnumerable<PriorityItem> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        PriorityItem firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Correct, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Incorrect, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Streak, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.DueAt, Is.Not.EqualTo(default(DateTime)));
        Assert.That(firstItem.Priority, Is.GreaterThan(0.0));
    }
}
