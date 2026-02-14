using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetBlindSpotsTests
{
    private CardboxDbContext _context;
    private GetBlindSpots _query;

    [SetUp]
    public void Setup()
    {
        _context = AnalyticsTestDataSetup.CreateAnalyticsTestContext();
        _query = new GetBlindSpots(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        AnalyticsTestDataSetup.CleanupAnalyticsTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnAccuracyByDifficultyAndLength()
    {
        IEnumerable<BlindSpotStats> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        BlindSpotStats firstSpot = result.First();
        Assert.That(firstSpot.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstSpot.Length, Is.GreaterThan(0));
        Assert.That(firstSpot.Items, Is.GreaterThan(0));
        Assert.That(firstSpot.PercentCorrect, Is.GreaterThanOrEqualTo(0.0));
        Assert.That(firstSpot.Reviews, Is.GreaterThanOrEqualTo(30));
    }
}
