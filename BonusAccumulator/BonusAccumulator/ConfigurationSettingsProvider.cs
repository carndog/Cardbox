using Microsoft.Extensions.Configuration;
using WordServices;

namespace BonusAccumulator;

public class ConfigurationSettingsProvider : ISettingsProvider
{
    private readonly IConfiguration _configuration;

    public ConfigurationSettingsProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GetSetting(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _configuration[key] ?? string.Empty;
    }
}
