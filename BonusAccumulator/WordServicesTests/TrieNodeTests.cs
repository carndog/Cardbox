using WordServices;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class TrieNodeTests
{
    [Test]
    public void Constructor_DefaultLabel_SetsLabelToNullChar()
    {
        TrieNode node = new TrieNode();

        node.Label.Should().Be('\0');
    }

    [Test]
    public void Constructor_CustomLabel_SetsLabelCorrectly()
    {
        TrieNode node = new TrieNode('A');

        node.Label.Should().Be('A');
    }

    [Test]
    public void Constructor_InitializesEdgesWithCapacity26()
    {
        TrieNode node = new TrieNode();

        node.Edges.Should().NotBeNull();
        node.Edges.Capacity.Should().Be(26);
        node.Edges.Should().BeEmpty();
    }

    [Test]
    public void Constructor_InitializesTerminalToFalse()
    {
        TrieNode node = new TrieNode();

        node.Terminal.Should().BeFalse();
    }

    [Test]
    public void Constructor_InitializesAnagramsAtTerminalWithCapacity4()
    {
        TrieNode node = new TrieNode();

        node.AnagramsAtTerminal.Should().NotBeNull();
        node.AnagramsAtTerminal.Capacity.Should().Be(4);
        node.AnagramsAtTerminal.Should().BeEmpty();
    }

    [Test]
    public void Terminal_SetToTrue_UpdatesTerminalProperty()
    {
        TrieNode node = new TrieNode
        {
            Terminal = true
        };

        node.Terminal.Should().BeTrue();
    }

    [Test]
    public void Terminal_SetToFalse_UpdatesTerminalProperty()
    {
        TrieNode node = new TrieNode
        {
            Terminal = true
        };

        node.Terminal = false;

        node.Terminal.Should().BeFalse();
    }

    [Test]
    public void Edges_CanAddChildNodes()
    {
        TrieNode parent = new TrieNode('P');
        TrieNode child = new TrieNode('C');

        parent.Edges.Add(child);

        parent.Edges.Should().HaveCount(1);
        parent.Edges[0].Should().Be(child);
        parent.Edges[0].Label.Should().Be('C');
    }

    [Test]
    public void Edges_CanAddMultipleChildNodes()
    {
        TrieNode parent = new TrieNode('P');
        TrieNode child1 = new TrieNode('A');
        TrieNode child2 = new TrieNode('B');
        TrieNode child3 = new TrieNode('C');

        parent.Edges.Add(child1);
        parent.Edges.Add(child2);
        parent.Edges.Add(child3);

        parent.Edges.Should().HaveCount(3);
        parent.Edges.Should().Contain(child1);
        parent.Edges.Should().Contain(child2);
        parent.Edges.Should().Contain(child3);
    }

    [Test]
    public void AnagramsAtTerminal_CanAddWords()
    {
        TrieNode node = new TrieNode();

        node.AnagramsAtTerminal.Add("CAT");
        node.AnagramsAtTerminal.Add("ACT");

        node.AnagramsAtTerminal.Should().HaveCount(2);
        node.AnagramsAtTerminal.Should().Contain("CAT");
        node.AnagramsAtTerminal.Should().Contain("ACT");
    }

    [Test]
    public void AnagramsAtTerminal_CanClearWords()
    {
        TrieNode node = new TrieNode();
        node.AnagramsAtTerminal.Add("WORD");
        node.AnagramsAtTerminal.Add("DROW");

        node.AnagramsAtTerminal.Clear();

        node.AnagramsAtTerminal.Should().BeEmpty();
    }

    [Test]
    public void MultipleNodes_EachHaveIndependentProperties()
    {
        TrieNode node1 = new TrieNode('A');
        TrieNode node2 = new TrieNode('B');

        node1.Terminal = true;
        node1.AnagramsAtTerminal.Add("WORD1");
        node1.Edges.Add(new TrieNode('C'));

        node2.Terminal = false;
        node2.AnagramsAtTerminal.Add("WORD2");

        node1.Label.Should().Be('A');
        node1.Terminal.Should().BeTrue();
        node1.AnagramsAtTerminal.Should().Contain("WORD1");
        node1.Edges.Should().HaveCount(1);

        node2.Label.Should().Be('B');
        node2.Terminal.Should().BeFalse();
        node2.AnagramsAtTerminal.Should().Contain("WORD2");
        node2.Edges.Should().BeEmpty();
    }
}
