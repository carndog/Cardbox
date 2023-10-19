namespace BonusAccumulator.WordServices.TrieLoading;

public interface ILazyLoadingTrie
{
    TrieNode? Lexicon { get; }
}