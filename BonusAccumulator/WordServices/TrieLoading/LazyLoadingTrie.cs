namespace WordServices.TrieLoading;

public class LazyLoadingTrie(IAnagramTrieBuilder anagramTrieBuilder) : ILazyLoadingTrie
{
    private Lazy<TrieNode?> LazyLexicon { get; } = new(anagramTrieBuilder.LoadLines);

    public TrieNode? Lexicon => LazyLexicon.Value;
}