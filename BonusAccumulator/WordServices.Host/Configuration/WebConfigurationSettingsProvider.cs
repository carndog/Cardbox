using Microsoft.Extensions.Configuration;
using WordServices;

namespace WordServices.Host.Configuration;

public class WebConfigurationSettingsProvider(IConfiguration configuration) : ISettingsProvider
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public string GetSetting(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _configuration[key] ?? string.Empty;
    }
}
