using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.TrieLoading;
using BonusAccumulator.WordServices.TrieSearching;
using static WordServicesTests.Utils;

namespace WordServicesTests;

[TestFixture]
public class TrieSearcherTests
{
    private WordService _wordService;

    [SetUp]
    public void SetUp()
    {
        _wordService = new WordService(new TrieSearcher(new LazyLoadingTrie(new AnagramTrieBuilder(TestFilePath, new TrieNode()))));
    }

    [Test]
    public void RunAnagramSearch()
    {
        Answer results = _wordService.Anagram("DGO");
        Assert.AreEqual(1, results.Words.Count);
        Assert.AreEqual("DOG", results.Words[0]);
    }

    [Test]
    public void RunPatternSearch()
    {
        Answer results = _wordService.Pattern("DOGGY");
        Assert.AreEqual("DOGGY", results.Words[0]);
    }

    [Test]
    public void RunBuildSearch()
    {
        Answer results = _wordService.Build("DOGGY");
        Assert.AreEqual(2, results.Words.Count);
        Assert.IsTrue(results.Words.Contains("DOG"));
        Assert.IsTrue(results.Words.Contains("DOGGY"));
    }

    [Test]
    public void RunBuildWithWildcardSearch()
    {
        Answer results = _wordService.Build("ZEBR.AS.");
        Assert.AreEqual(5, results.Words.Count);
        Assert.IsTrue(results.Words.Contains("ZEBRASS"));
        Assert.IsTrue(results.Words.Contains("ZEBRA"));
        Assert.IsTrue(results.Words.Contains("ZOO"));
        Assert.IsTrue(results.Words.Contains("CAT"));
        Assert.IsTrue(results.Words.Contains("ACT"));
    }


    [Test]
    public void RunPatternWithWildcardSearch()
    {
        Answer results = _wordService.Pattern("ZEB.AS.");
        Assert.AreEqual(1, results.Words.Count);
        Assert.IsTrue(results.Words.Contains("ZEBRASS"));
    }


    [Test]
    public void RunAnagramWithWildcardSearch()
    {
        Answer results = _wordService.Anagram("ZEBRA..");
        Assert.AreEqual(1, results.Words.Count);
        Assert.IsTrue(results.Words.Contains("ZEBRASS"));
    }
    
    [TestCase("tipula", "TIPUNA")]
    [TestCase("tupuna", "TIPUNA")]
    public void RunAlphagramDistanceSearch(string search, string match)
    {
        Answer results = _wordService.Distance(search);
        Assert.AreEqual(2, results.Words.Count);
        Assert.IsTrue(results.Words.Contains(search.ToUpper()));
        Assert.IsTrue(results.Words.Contains(match));
    }

    [Test]
    public void RunAlphagramDistanceWildcardSearch()
    {
        Answer results = _wordService.Distance("TIPU.A");
        Assert.AreEqual(3, results.Words.Count);
        Assert.IsTrue(results.Words.Contains("TIPULA"));
        Assert.IsTrue(results.Words.Contains("TIPUNA"));
        Assert.IsTrue(results.Words.Contains("TUPUNA"));
    }
}