using Microsoft.Extensions.Configuration;

namespace BonusAccumulator;

public static class Configuration
{
    public static string? GetSetting(string setting)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");
        
        var configuration = builder.Build();

        string? result = configuration[setting];

        return result;
    }
}