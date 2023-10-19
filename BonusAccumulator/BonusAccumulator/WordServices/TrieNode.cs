namespace BonusAccumulator.WordServices;

public class TrieNode
{
    public List<TrieNode?> Edges { get; init; } = new();

    public char Label { get; init; }

    public bool Terminal { get; set; }

    public List<string> AnagramsAtTerminal { get; } = new();
}