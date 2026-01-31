using BonusAccumulator.WordServices.Output;
using BonusAccumulator.WordServices.TrieLoading;
using BonusAccumulator.WordServices.TrieSearching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BonusAccumulator.WordServices;

public static class DependencyInjection
{
    public static IServiceCollection AddWordServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddSingleton<ISettingsProvider, SettingsProvider>();
        services.AddSingleton<IWordOutputService, DefaultWordOutputService>();
        services.AddSingleton<TrieNode>();
        services.AddSingleton<IAnagramTrieBuilder>(provider => 
            new AnagramTrieBuilder(
                provider.GetRequiredService<ISettingsProvider>().GetSetting("DictionaryPath") ?? string.Empty,
                provider.GetRequiredService<TrieNode>()));
        services.AddSingleton<ILazyLoadingTrie, LazyLoadingTrie>();
        services.AddSingleton<ITrieSearcher, TrieSearcher>();
        services.AddSingleton<ISessionState, SessionState>();
        services.AddSingleton<IWordService, WordService>();

        return services;
    }
}
