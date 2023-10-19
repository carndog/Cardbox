using BonusAccumulator.WordServices.TrieLoading;
using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices.Factories;

public static class WordServiceFactory
{
    public static WordService Create()
    {
        string? lexiconListTxt = new SettingsProvider().GetSetting("DictionaryPath");
        return new WordService(new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(
                lexiconListTxt, new TrieNode()))));
    }
}