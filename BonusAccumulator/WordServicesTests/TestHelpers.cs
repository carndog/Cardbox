using BonusAccumulator;
using static WordServicesTests.Utils;

namespace WordServicesTests;

public class TestSettingsProvider : ISettingsProvider
{
    private readonly string _sessionOutputPath;

    public TestSettingsProvider(string sessionOutputPath)
    {
        _sessionOutputPath = sessionOutputPath;
    }

    public string? GetSetting(string key)
    {
        return key switch
        {
            "SessionOutputPath" => _sessionOutputPath,
            "DictionaryPath" => TestFilePath,
            _ => null
        };
    }
}
