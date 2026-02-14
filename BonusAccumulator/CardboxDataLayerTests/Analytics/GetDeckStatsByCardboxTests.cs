using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetDeckStatsByCardboxTests
{
    private CardboxDbContext _context;
    private GetDeckStatsByCardbox _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetDeckStatsByCardbox(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnStatsByCardbox()
    {
        IEnumerable<CardboxStats> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        CardboxStats firstCardbox = result.First();
        Assert.That(firstCardbox.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstCardbox.Items, Is.GreaterThan(0));
        Assert.That(firstCardbox.TotalReviews, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstCardbox.PctCorrect, Is.GreaterThanOrEqualTo(0.0));
    }
}
