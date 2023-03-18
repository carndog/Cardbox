namespace BonusAccumulator.WordServices;

public class LazyLoadingTrie
{
    public LazyLoadingTrie(AnagramTrieBuilder anagramTrieBuilder)
    {
        LazyLexicon = new Lazy<TrieNode?>(anagramTrieBuilder.LoadLines);
    }

    private Lazy<TrieNode?> LazyLexicon { get; }

    public TrieNode? Lexicon => LazyLexicon.Value;
}