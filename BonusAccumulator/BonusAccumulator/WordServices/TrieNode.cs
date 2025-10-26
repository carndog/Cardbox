namespace BonusAccumulator.WordServices;

public class TrieNode
{
    public List<TrieNode> Edges { get; init; } = new(capacity: 26);
    
    public char Label { get; init; }
    
    public bool Terminal { get; set; }
    
    public List<string> AnagramsAtTerminal { get; init; } = new(capacity: 4);

    public TrieNode(char label = default)
    {
        Label = label;
    }
}