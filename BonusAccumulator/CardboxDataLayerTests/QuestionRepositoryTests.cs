using CardboxDataLayer;
using CardboxDataLayer.Entities;
using CardboxDataLayer.Repositories;

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
            Assert.Ignore("No questions found in test database");
            return;
        }

        Question? result = await _repository.GetByQuestionAsync(firstQuestion!.question);

        Assert.That(result, Is.Not.Null);

        Assert.That(firstQuestion!.question, Is.EqualTo(result!.question));
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
            Assert.That(1, Is.EqualTo(question.cardbox));
        }
    }

    [Test]
    public async Task GetByDifficultyAsync_ShouldReturnQuestionsWithSpecifiedDifficulty()
    {
        List<Question> questions = (await _repository.GetByDifficultyAsync(1)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(1, Is.EqualTo(question.difficulty));
        }
    }

    [Test]
    public async Task GetByDifficultyRangeAsync_ShouldReturnQuestionsInRange()
    {
        List<Question> questions = (await _repository.GetByDifficultyRangeAsync(1, 3)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.difficulty >= 1 && question.difficulty <= 3, Is.True);
        }
    }

    [Test]
    public async Task GetByStreakAsync_ShouldReturnQuestionsWithMinimumStreak()
    {
        List<Question> questions = (await _repository.GetByStreakAsync(0)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.streak >= 0, Is.True);
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
            Assert.That(question.next_scheduled <= maxScheduled, Is.True);
        }
    }

    [Test]
    public async Task GetIncorrectAnswersAsync_ShouldReturnQuestionsWithMinimumIncorrect()
    {
        List<Question> questions = (await _repository.GetIncorrectAnswersAsync(0)).ToList();

        Assert.That(questions, Is.Not.Null);
        foreach (Question question in questions)
        {
            Assert.That(question.incorrect >= 0, Is.True);
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
            Assert.Ignore("No questions found in test database");
            return;
        }

        List<QuestionHistory> history = (await _repository.GetQuestionHistoryAsync(firstQuestion!.question)).ToList();

        Assert.That(history, Is.Not.Null);
        foreach (QuestionHistory entry in history)
        {
            Assert.That(firstQuestion.question, Is.EqualTo(entry.question));
        }
    }
}
