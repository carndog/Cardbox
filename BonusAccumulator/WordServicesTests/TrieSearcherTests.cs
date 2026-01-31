using WordServices;
using WordServices.TrieLoading;
using WordServices.Output;
using WordServices.TrieSearching;
using static WordServicesTests.Utils;

namespace WordServicesTests;

[TestFixture]
public class TrieSearcherTests
{
    private WordService _wordService = null!;

    [SetUp]
    public void SetUp()
    {
        _wordService = new WordService(new TrieSearcher(new LazyLoadingTrie(new AnagramTrieBuilder(TestFilePath, new TrieNode()))), new SessionState(new TestSettingsProvider(Path.GetTempPath())), new DefaultWordOutputService());
    }

    [Test]
    public void RunAnagramSearch()
    {
        Answer results = _wordService.Anagram("DGO");
        Assert.That(results.Words.Count, Is.EqualTo(1));
        Assert.That(results.Words[0], Is.EqualTo("DOG"));
    }

    [Test]
    public void RunPatternSearch()
    {
        Answer results = _wordService.Pattern("DOGGY");
        Assert.That(results.Words[0], Is.EqualTo("DOGGY"));
    }

    [Test]
    public void RunBuildSearch()
    {
        Answer results = _wordService.Build("DOGGY");
        Assert.That(results.Words.Count, Is.EqualTo(2));
        Assert.That(results.Words.Contains("DOG"));
        Assert.That(results.Words.Contains("DOGGY"));
    }

    [Test]
    public void RunBuildWithWildcardSearch()
    {
        Answer results = _wordService.Build("ZEBR.AS.");
        Assert.That(results.Words.Count, Is.EqualTo(6));
        Assert.That(results.Words.Contains("ZEBRASS"));
        Assert.That(results.Words.Contains("ZEBRA"));
        Assert.That(results.Words.Contains("ZOO"));
        Assert.That(results.Words.Contains("CAT"));
        Assert.That(results.Words.Contains("ACT"));
        Assert.That(results.Words.Contains("TWA"));
    }


    [Test]
    public void RunPatternWithWildcardSearch()
    {
        Answer results = _wordService.Pattern("ZEB.AS.");
        Assert.That(results.Words.Count, Is.EqualTo(1));
        Assert.That(results.Words.Contains("ZEBRASS"));
    }

    [Test]
    public void RunAnagramWithWildcardSearch()
    {
        Answer results = _wordService.Anagram("ZEBRA..");
        Assert.That(results.Words.Count, Is.EqualTo(1));
        Assert.That(results.Words.Contains("ZEBRASS"));
    }
    
    [TestCase("tipula", "TIPUNA")]
    [TestCase("tupuna", "TIPUNA")]
    public void RunDistanceSearch(string search, string match)
    {
        Answer results = _wordService.Distance(search);
        Assert.That(results.Words.Count, Is.EqualTo(2));
        Assert.That(results.Words.Contains(search.ToUpper()));
        Assert.That(results.Words.Contains(match));
    }

    [Test]
    public void RunDistanceWildcardSearch()
    {
        Answer results = _wordService.Distance("TIPU.A");
        Assert.That(results.Words.Count, Is.EqualTo(3));
        Assert.That(results.Words.Contains("TIPULA"));
        Assert.That(results.Words.Contains("TIPUNA"));
        Assert.That(results.Words.Contains("TUPUNA"));
    }

    [Test]
    public void RunWithAlphagramDistance()
    {
        Answer results = _wordService.AlphagramDistance("act");
        Assert.That(results.Words.Count, Is.EqualTo(3));
        Assert.That(results.Words.Contains("ACT"));
        Assert.That(results.Words.Contains("CAT"));
        Assert.That(results.Words.Contains("TWA"));
    }
}