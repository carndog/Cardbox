using BonusAccumulator.WordServices.Extensions;

namespace BonusAccumulator.WordServices.TrieLoading;

public class AnagramTrieBuilder : IAnagramTrieBuilder
{
    private readonly TrieNode? _item;
    private readonly string? _path;
    
    public AnagramTrieBuilder(string? filePath, TrieNode? item)
    {
        _path = filePath;
        _item = item;
    }

    public TrieNode? LoadLines()
    {
        if (!File.Exists(_path))
        {
            throw new FileNotFoundException("Dictionary file not found.", _path);
        }

        using FileStream fileStream = new(_path, FileMode.Open);
        using StreamReader reader = new(fileStream);
        while (reader.ReadLine() is { } line)
        {
            LoadLine(_item, line);
        }

        return _item;
    }

    private void LoadLine(TrieNode? root, string? line)
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
        if (current != null && current.Edges.All(edge => edge.Label != c))
        {
            next = new TrieNode { Label = c, Edges = new List<TrieNode>() };
            current.Edges.Add(next);
        }
        else
        {
            next = current?.Edges.First(edge => edge.Label == c);
        }

        return next;
    }
}