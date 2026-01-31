namespace WordServices.TrieLoading;

public interface ILazyLoadingTrie
{
    TrieNode? Lexicon { get; }
}