using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetDeckStatsByWordLengthTests
{
    private CardboxDbContext _context = null!;
    private GetDeckStatsByWordLength _query = null!;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetDeckStatsByWordLength(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnStatsByLength()
    {
        IEnumerable<WordLengthStats> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        WordLengthStats firstLength = result.First();
        Assert.That(firstLength.Length, Is.GreaterThan(0));
        Assert.That(firstLength.Items, Is.GreaterThan(0));
        Assert.That(firstLength.PercentCorrect, Is.GreaterThanOrEqualTo(0.0));
    }

    [Test]
    public async Task ExecuteAsync_ShouldExcludeInactiveWords()
    {
        IEnumerable<WordLengthStats> result = await _query.ExecuteAsync();

        int totalItemsInResults = result.Sum(stat => stat.Items);
        int activeWords = _context.Questions.Count(q => q.Cardbox != null);

        Assert.That(totalItemsInResults, Is.EqualTo(activeWords));
    }
}
