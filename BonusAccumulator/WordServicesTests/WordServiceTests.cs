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
    public void RunChainQuiz_SingleLetterReplacement_ShouldContinueSameChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CAT", "CAR", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CAT");
        output.Should().NotContain("🔗 New chain starting from: CAR");
    }

    [Test]
    public void RunChainQuiz_SingleLetterAddition_ShouldContinueSameChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CAT", "CART", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CAT");
        output.Should().NotContain("🔗 New chain starting from: CART");
    }

    [Test]
    public void RunChainQuiz_SingleLetterRemoval_ShouldContinueSameChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CART", "CAT", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CART");
        output.Should().NotContain("🔗 New chain starting from: CAT");
    }

    [Test]
    public void RunChainQuiz_MultipleLetterChanges_ShouldStartNewChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CAT", "DOG", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CAT");
        output.Should().Contain("🔗 New chain starting from: DOG");
    }

    [Test]
    public void RunChainQuiz_MultipleLetterAddition_ShouldStartNewChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CAT", "CARTS", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CAT");
        output.Should().Contain("🔗 New chain starting from: CARTS");
    }

    [Test]
    public void RunChainQuiz_Anagrams_ShouldContinueSameChain()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["CAT", "ACT", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("🔗 Starting chain from: CAT");
        output.Should().NotContain("🔗 New chain starting from: ACT");
    }

    [Test]
    public void RunChainQuiz_NonWord_ShouldShowErrorMessage()
    {
        List<string> output = [];
        Queue<string?> inputQueue = new(["XYZ123", "xch"]);
        
        Action<string?> write = s => output.Add(s ?? string.Empty);
        Func<string?> read = () => inputQueue.Count > 0 ? inputQueue.Dequeue() : "xch";

        _service.RunChainQuiz("xch", write, read);

        output.Should().Contain("'XYZ123' is not a valid word or has no anagrams");
    }

    [Test]
    public void AddLastWords_WithValidLastResult_ShouldAddWordsAndShowFeedback()
    {
        _service.Anagram("CAT");
        
        List<string> output = [];
        Action<string> write = s => output.Add(s ?? string.Empty);

        _service.AddLastWords(write);

        output.Should().Contain("Added 2 words from last result:");
        output.Should().Contain("(ACT,CAT)");
    }

    [Test]
    public void AddLastWords_WithNoLastResult_ShouldShowNoWordsMessage()
    {
        List<string> output = [];
        Action<string> write = s => output.Add(s ?? string.Empty);

        _service.AddLastWords(write);

        output.Should().Contain("No words to add from last result.");
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
