using Microsoft.Extensions.Configuration;

namespace BonusAccumulator;

public class SettingsProvider : ISettingsProvider
{
    private readonly IConfiguration _configuration;

    public SettingsProvider(string appSettingsPath = "appsettings.json")
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);

        _configuration = builder.Build();
    }

    public string? GetSetting(string key)
    {
        return _configuration[key];
    }
}