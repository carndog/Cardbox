namespace BonusAccumulator.WordServices;

public class TrieNode
{
    public List<TrieNode?> Edges { get; set; } = new();

    public char Label { get; set; }

    public bool Terminal { get; set; }

    public List<string> AnagramsAtTerminal { get; } = new();
}