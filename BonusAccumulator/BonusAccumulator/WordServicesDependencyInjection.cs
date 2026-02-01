using Microsoft.Extensions.DependencyInjection;
using WordServices;
using WordServices.Output;
using WordServices.TrieLoading;
using WordServices.TrieSearching;

namespace BonusAccumulator;

public static class WordServicesDependencyInjection
{
    public static void AddWordServices(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsProvider, ConfigurationSettingsProvider>();
        services.AddSingleton<IWordOutputService, DefaultWordOutputService>();
        services.AddSingleton<TrieNode>();
        services.AddSingleton<IAnagramTrieBuilder>(provider => 
            new AnagramTrieBuilder(
                provider.GetRequiredService<ISettingsProvider>().GetSetting("DictionaryPath"),
                provider.GetRequiredService<TrieNode>()));
        services.AddSingleton<ILazyLoadingTrie, LazyLoadingTrie>();
        services.AddSingleton<ITrieSearcher, TrieSearcher>();
        services.AddSingleton<ISessionState, SessionState>();
        services.AddSingleton<IWordService, WordService>();
    }
}
