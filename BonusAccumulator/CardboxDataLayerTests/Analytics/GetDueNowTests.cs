using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetDueNowTests
{
    private CardboxDbContext _context;
    private GetDueNow _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetDueNow(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnOverdueItems()
    {
        IEnumerable<DueItem> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        DueItem firstDue = result.First();
        Assert.That(firstDue.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstDue.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstDue.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstDue.DueAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }
}
