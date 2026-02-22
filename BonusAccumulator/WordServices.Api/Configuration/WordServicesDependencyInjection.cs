using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WordServices;
using WordServices.Output;
using WordServices.TrieLoading;
using WordServices.TrieSearching;

namespace WordServices.Api.Configuration;

public static class WordServicesDependencyInjection
{
    public static void AddWordServicesForWeb(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ISettingsProvider>(new WebConfigurationSettingsProvider(configuration));
        services.AddSingleton<IWordOutputService, DefaultWordOutputService>();
        services.AddSingleton<TrieNode>();
        services.AddSingleton<IAnagramTrieBuilder>(provider =>
            new AnagramTrieBuilder(
                provider.GetRequiredService<ISettingsProvider>().GetSetting("DictionaryPath"),
                provider.GetRequiredService<TrieNode>()));
        services.AddSingleton<ILazyLoadingTrie, LazyLoadingTrie>();
        services.AddSingleton<ITrieSearcher, TrieSearcher>();
        services.AddScoped<ISessionState, SessionState>();
        services.AddScoped<IWordService, WordService>();
    }
}
