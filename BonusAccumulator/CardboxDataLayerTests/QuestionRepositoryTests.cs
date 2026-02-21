using CardboxDataLayer;
using CardboxDataLayer.Entities;
using CardboxDataLayer.Repositories;
using WordServices.Analytics;
using CardboxDataLayerTests.TestHelpers;

namespace CardboxDataLayerTests;

[TestFixture]
public class QuestionRepositoryTests
{
    private CardboxDbContext _context = null!;
    private IQuestionRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        _context = TestDatabaseSetup.CreateTestContext();
        _repository = new QuestionRepository(_context);
    }

    [TearDown]
    public void Cleanup()
    {
        _context?.Dispose();
        TestDatabaseSetup.CleanupTestDatabase();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnQuestions()
    {
        List<Question> questions = (await _repository.GetAllAsync()).ToList();

        Assert.That(questions, Is.Not.Null);
    }

    [Test]
    public async Task GetByQuestionAsync_WithValidQuestion_ShouldReturnQuestion()
    {
        List<Question> allQuestions = (await _repository.GetAllAsync()).ToList();
        Question? firstQuestion = allQuestions.FirstOrDefault();
        
        if (firstQuestion == null)
        {
            Assert.Ignore();
            return;
        }

        Question? result = await _repository.GetByQuestionAsync(firstQuestion!.QuestionText);

        Assert.That(result, Is.Not.Null);

        Assert.That(firstQuestion!.QuestionText, Is.EqualTo(result!.QuestionText));
    }

    [Test]
    public async Task GetByQuestionAsync_WithInvalidQuestion_ShouldReturnNull()
    {
        Question? result = await _repository.GetByQuestionAsync("NONEXISTENT_QUESTION");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByCardboxAsync_ShouldReturnQuestionsInCardbox()
    {
        List<Question> questions = (await _repository.GetByCardboxAsync(1)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(1, Is.EqualTo(question.Cardbox));
        }
    }

    [Test]
    public async Task GetByDifficultyAsync_ShouldReturnQuestionsWithSpecifiedDifficulty()
    {
        List<Question> questions = (await _repository.GetByDifficultyAsync(1)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(1, Is.EqualTo(question.Difficulty));
        }
    }

    [Test]
    public async Task GetByDifficultyRangeAsync_ShouldReturnQuestionsInRange()
    {
        List<Question> questions = (await _repository.GetByDifficultyRangeAsync(1, 3)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.Difficulty >= 1 && question.Difficulty <= 3, Is.True);
        }
    }

    [Test]
    public async Task GetByStreakAsync_ShouldReturnQuestionsWithMinimumStreak()
    {
        List<Question> questions = (await _repository.GetByStreakAsync(0)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.Streak >= 0, Is.True);
        }
    }

    [Test]
    public async Task GetScheduledAsync_ShouldReturnScheduledQuestions()
    {
        int maxScheduled = int.MaxValue;

        List<Question> questions = (await _repository.GetScheduledAsync(maxScheduled)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.NextScheduled <= maxScheduled, Is.True);
        }
    }

    [Test]
    public async Task GetIncorrectAnswersAsync_ShouldReturnQuestionsWithMinimumIncorrect()
    {
        List<Question> questions = (await _repository.GetIncorrectAnswersAsync(0)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.Incorrect >= 0, Is.True);
        }
    }

    [Test]
    public async Task GetTotalCountAsync_ShouldReturnCorrectCount()
    {
        int count = await _repository.GetTotalCountAsync();

        Assert.That(count >= 0, Is.True);
    }

    [Test]
    public async Task GetCountByCardboxAsync_ShouldReturnCorrectCount()
    {
        int count = await _repository.GetCountByCardboxAsync(1);

        Assert.That(count >= 0, Is.True);
    }

    [Test]
    public async Task GetAverageDifficultyAsync_ShouldReturnValidAverage()
    {
        double average = await _repository.GetAverageDifficultyAsync();

        Assert.That(average >= 0, Is.True);
    }

    [Test]
    public async Task GetQuestionHistoryAsync_ShouldReturnHistoryForQuestion()
    {
        List<Question> allQuestions = (await _repository.GetAllAsync()).ToList();
        Question? firstQuestion = allQuestions.FirstOrDefault();
        
        if (firstQuestion == null)
        {
            Assert.Ignore();
            return;
        }

        List<QuestionHistory> history = (await _repository.GetQuestionHistoryAsync(firstQuestion!.QuestionText)).ToList();

        Assert.That(history, Is.Not.Null);
        foreach (QuestionHistory entry in history)
        {
            Assert.That(firstQuestion.QuestionText, Is.EqualTo(entry.QuestionText));
        }
    }

    [Test]
    public async Task GetQuestionsByAlphagramLengthAsync_ShouldReturnStatsGroupedByLength()
    {
        List<AlphagramLengthStats> result = (await _repository.GetQuestionsByAlphagramLengthAsync()).ToList();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        
        for (int i = 1; i < result.Count; i++)
        {
            Assert.That(result[i].AlphagramLength, Is.GreaterThanOrEqualTo(result[i - 1].AlphagramLength));
        }
        
        foreach (AlphagramLengthStats stat in result)
        {
            Assert.That(stat.AlphagramLength, Is.GreaterThan(0));
            Assert.That(stat.Questions, Is.GreaterThan(0));
        }
    }

    [Test]
    public async Task GetQuestionsByAlphagramLengthAsync_ShouldMatchTotalQuestions()
    {
        List<AlphagramLengthStats> alphagramStats = (await _repository.GetQuestionsByAlphagramLengthAsync()).ToList();
        int totalQuestions = await _repository.GetTotalCountAsync();

        int questionsInStats = alphagramStats.Sum(stat => stat.Questions);

        Assert.That(questionsInStats, Is.EqualTo(totalQuestions));
    }

    [Test]
    public async Task GetQuestionsByAlphagramLengthAsync_ShouldGroupCorrectly()
    {
        List<Question> allQuestions = (await _repository.GetAllAsync()).ToList();
        List<AlphagramLengthStats> result = (await _repository.GetQuestionsByAlphagramLengthAsync()).ToList();

        List<ManualGroupingResult> manualGrouping = allQuestions
            .GroupBy(q => q.QuestionText.Length)
            .OrderBy(g => g.Key)
            .Select(g => new ManualGroupingResult(g.Key, g.Count()))
            .ToList();

        Assert.That(result.Count, Is.EqualTo(manualGrouping.Count));

        for (int i = 0; i < result.Count; i++)
        {
            Assert.That(result[i].AlphagramLength, Is.EqualTo(manualGrouping[i].Length));
            Assert.That(result[i].Questions, Is.EqualTo(manualGrouping[i].Count));
        }
    }
}
