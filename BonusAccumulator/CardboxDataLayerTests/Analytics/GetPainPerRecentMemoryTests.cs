using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetPainPerRecentMemoryTests
{
    private CardboxDbContext _context;
    private GetPainPerRecentMemory _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetPainPerRecentMemory(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsWithLowStreakAndHighIncorrect()
    {
        IEnumerable<PainStats> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        PainStats firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Incorrect, Is.GreaterThanOrEqualTo(8));
        Assert.That(firstItem.Correct, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Streak, Is.LessThanOrEqualTo(2));
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
    }
}
