using BonusAccumulator.WordServices.TrieLoading;
using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices.Factories;

public static class WordServiceFactory
{
    public static WordService Create()
    {
        const string lexiconListTxt = @"C:\Lexicon\list.txt";
        return new WordService(new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(
                lexiconListTxt, new TrieNode()))));
    }
}