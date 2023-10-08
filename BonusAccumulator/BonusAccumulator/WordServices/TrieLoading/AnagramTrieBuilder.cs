using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices.TrieLoading;

public class AnagramTrieBuilder
{
    private readonly TrieNode? _item;

    public AnagramTrieBuilder(string filePath, TrieNode? item)
    {
        Path = filePath;
        _item = item;
    }

    public string Path { get; }

    public TrieNode? LoadLines()
    {
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException(Path);
        }

        using (var fileStream = new FileStream(Path, FileMode.Open))
        using (var reader = new StreamReader(fileStream))
        {
            while (reader.ReadLine() is { } line)
            {
                LoadLine(_item, line);
            }
        }
        return _item;
    }

    public void LoadLine(TrieNode? root, string? line)
    {
        TrieNode? current = root;
        string? word = line?.Trim();
        
        if(word is null) 
            return;
        
        char[] chars = word.ToAlphagram().ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];

            TrieNode? next = AddCurrentCharacterToTrie(current, c);

            bool isTerminal = i == chars.Length - 1;
            if (isTerminal)
            {
                AssignWordToNode(next, word);
            }
            current = next;
        }
    }

    private static void AssignWordToNode(TrieNode? next, string word)
    {
        if (next != null)
        {
            next.Terminal = true;
            CollectAnagrams(next, word);
        }
    }

    private static void CollectAnagrams(TrieNode? next, string word)
    {
        next?.AnagramsAtTerminal.Add(word);
    }

    private static TrieNode? AddCurrentCharacterToTrie(TrieNode? current, char c)
    {
        TrieNode? next;
        if (current != null && current.Edges.All(edge => edge != null && edge.Label != c))
        {
            next = new TrieNode { Label = c, Edges = new List<TrieNode?>() };
            current.Edges.Add(next);
        }
        else
        {
            next = current?.Edges.First(edge => edge != null && edge.Label == c);
        }

        return next;
    }
}