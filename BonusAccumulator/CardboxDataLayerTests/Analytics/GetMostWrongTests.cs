using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetMostWrongTests
{
    private CardboxDbContext _context;
    private GetMostWrong _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetMostWrong(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsWithMostIncorrectAnswers()
    {
        IEnumerable<MostWrongStats> result = await _query.ExecuteAsync(10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        MostWrongStats firstItem = result.First();
        Assert.That(firstItem.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstItem.Incorrect, Is.GreaterThan(0));
        Assert.That(firstItem.Correct, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Attempts, Is.GreaterThan(0));
        Assert.That(firstItem.Streak, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstItem.Difficulty, Is.GreaterThanOrEqualTo(0));
    }
}
