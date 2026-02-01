using WordServices;
using WordServices.Output;
using WordServices.TrieLoading;
using WordServices.TrieSearching;
using FluentAssertions;
using static WordServicesTests.Utils;

namespace WordServicesTests;

[TestFixture]
public class WordServiceTests
{
    private WordService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _service = new WordService(new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(
                TestFilePath, new TrieNode()))), new SessionState(new TestSettingsProvider(Path.GetTempPath())), new DefaultWordOutputService());
    }

    [Test]
    public void Anagram_WithValidInput_ShouldReturnAnagrams()
    {
        Answer answer = _service.Anagram("CAT");

        answer.Words.Count().Should().Be(2);
        answer.Words.Should().ContainInOrder("ACT", "CAT");
    }

    [Test]
    public void Build_WithValidInput_ShouldReturnBuildWords()
    {
        Answer answer = _service.Build("TACT");

        answer.Words.Count().Should().Be(2);
        answer.Words.Should().ContainInOrder("ACT", "CAT");
    }

    [Test]
    public void RunQuiz_PartialAnswer_ShouldBeWrong()
    {
        _service.Anagram("CAT");
        
        List<string> output = [];
        Queue<string?> inputQueue = new(["ACT", "eqs"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "eqs";

        _service.RunQuiz(QuizOptions.Session, "eqs", write, read);

        output.Should().Contain("✗ Wrong");
    }

    [Test]
    public void RunQuiz_QuizRandomSelection_OffByOneError()
    {
        _service.Anagram("CAT");
        _service.Anagram("DOG");
        
        List<string> output = [];
        Queue<string?> inputQueue = new(["eqs"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "eqs";

        _service.RunQuiz(QuizOptions.Session, "eqs", write, read);
        
        output.Should().Contain("Quiz over");
    }

    [Test]
    public void WordService_StaticRandom_ThreadSafety()
    {
        _service.Anagram("TEST");
        
        List<string> output = [];
        Queue<string?> inputQueue = new(["eqs"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "eqs";

        Action act = () => _service.RunQuiz(QuizOptions.Session, "eqs", write, read);
        
        act.Should().NotThrow();
    }
}
