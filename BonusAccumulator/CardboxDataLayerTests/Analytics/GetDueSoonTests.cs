using CardboxDataLayer;
using CardboxDataLayer.Analytics;
using WordServices.Analytics;

namespace CardboxDataLayerTests.Analytics;

[TestFixture]
public class GetDueSoonTests
{
    private CardboxDbContext _context = null!;
    private GetDueSoon _query = null!;

    [SetUp]
    public void Setup()
    {
        _context = GetDueSoonTestDataSetup.CreateTestContext();
        _query = new GetDueSoon(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        GetDueSoonTestDataSetup.CleanupTestDatabase();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnItemsDueWithin24Hours()
    {
        IEnumerable<DueItem> result = await _query.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        
        DueItem firstDue = result.First();
        Assert.That(firstDue.Question, Is.Not.Null.And.Not.Empty);
        Assert.That(firstDue.Cardbox, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstDue.Difficulty, Is.GreaterThanOrEqualTo(0));
        Assert.That(firstDue.DueAt, Is.LessThanOrEqualTo(DateTime.UtcNow.AddHours(24)));
        
        List<string> dueQuestions = result.Select(item => item.Question).ToList();
        Assert.That(dueQuestions, Does.Contain("DUESOON1"));
        Assert.That(dueQuestions, Does.Contain("DUESOON2"));
        Assert.That(dueQuestions, Does.Contain("DUESOON3"));
        Assert.That(dueQuestions, Does.Not.Contain("OVERDUE1"));
        Assert.That(dueQuestions, Does.Not.Contain("FUTURE1"));
    }
}
