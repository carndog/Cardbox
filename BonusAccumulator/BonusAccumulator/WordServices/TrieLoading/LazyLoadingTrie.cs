namespace BonusAccumulator.WordServices.TrieLoading;

public class LazyLoadingTrie : ILazyLoadingTrie
{
    public LazyLoadingTrie(IAnagramTrieBuilder anagramTrieBuilder)
    {
        LazyLexicon = new Lazy<TrieNode?>(anagramTrieBuilder.LoadLines);
    }

    private Lazy<TrieNode?> LazyLexicon { get; }

    public TrieNode? Lexicon => LazyLexicon.Value;
}