using WordServices;

namespace WordServicesTests;

[TestFixture]
public class TrieLoaderTests
{
    private readonly Utils _utils = new Utils();

    [SetUp]
    public void SetUp()
    {
        _utils.CreateTrieLoader();
    }

    [Test]
    public void LoadTrie_IndividualWordsOnSeparateLinesInAFile_ProducesAnagramNodeStructure()
    {
        TrieNode? rootNode = _utils.AnagramTrieBuilder?.LoadLines();

        Assert.That(rootNode, Is.Not.Null);
        Assert.That(rootNode!.Edges.Count, Is.EqualTo(7));
        Assert.That(rootNode.Edges[1].Terminal, Is.False);
        Assert.That(rootNode.Edges[0].Label, Is.EqualTo('A'));
        Assert.That(rootNode.Edges[0].Edges[0].Edges[0].Terminal, Is.True);
        Assert.That(rootNode.Edges[0].Edges[0].Edges[0].AnagramsAtTerminal.Count, Is.EqualTo(2));
        Assert.That(rootNode.Edges[0].Edges[0].Edges[0].AnagramsAtTerminal.Contains("CAT"), Is.True);
    }
}