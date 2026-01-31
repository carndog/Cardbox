using BonusAccumulator.WordServices.Output;
using BonusAccumulator.WordServices.TrieLoading;
using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices.Factories;

public static class WordServiceFactory
{
    public static WordService Create()
    {
        SettingsProvider settingsProvider = new SettingsProvider();
        string? lexiconListTxt = settingsProvider.GetSetting("DictionaryPath");
        IWordOutputService wordOutputService = WordOutputServiceFactory.Create();
        return new WordService(new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(
                lexiconListTxt, new TrieNode()))), new SessionState(settingsProvider), wordOutputService);
    }
}