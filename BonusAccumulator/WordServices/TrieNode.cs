namespace WordServices;

public class TrieNode(char label = '\0')
{
    public List<TrieNode> Edges { get; init; } = new(capacity: 26);
    
    public char Label { get; init; } = label;

    public bool Terminal { get; set; }
    
    public List<string> AnagramsAtTerminal { get; } = new(capacity: 4);
}