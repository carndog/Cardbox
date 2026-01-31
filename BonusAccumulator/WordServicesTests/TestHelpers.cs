using WordServices;
using static WordServicesTests.Utils;

namespace WordServicesTests;

public class TestSettingsProvider : ISettingsProvider
{
    private readonly string _sessionOutputPath;
    private readonly string? _configPath;

    public TestSettingsProvider(string sessionOutputPath)
    {
        _sessionOutputPath = sessionOutputPath;
    }

    public TestSettingsProvider(string configPath, bool useConfigFile)
    {
        if (!string.IsNullOrEmpty(configPath) && !File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file not found: {configPath}");
        }
        _configPath = configPath;
    }

    public string? GetSetting(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        // For testing purposes, simulate configuration reading
        if (!string.IsNullOrEmpty(_configPath) && File.Exists(_configPath))
        {
            string json = File.ReadAllText(_configPath);
            
            // Simple JSON parsing for test cases
            if (json.Contains("\"DictionaryPath\""))
            {
                if (key == "DictionaryPath") return "test/dictionary.txt";
            }
            if (json.Contains("\"OtherSetting\""))
            {
                if (key == "OtherSetting") return "otherValue";
            }
            if (json.Contains("\"TestKey\""))
            {
                if (key == "TestKey") return "TestValue";
            }
            if (json.Contains("\"PortNumber\""))
            {
                if (key == "PortNumber") return "8080";
            }
            if (json.Contains("\"FeatureEnabled\""))
            {
                if (key == "FeatureEnabled") return "True";
            }
            if (json.Contains("\"Settings\"") && json.Contains("\"DictionaryPath\""))
            {
                if (key == "Settings:DictionaryPath") return "nested/dictionary.txt";
            }
        }

        return key switch
        {
            "SessionOutputPath" => _sessionOutputPath,
            "DictionaryPath" => TestFilePath,
            _ => null
        };
    }
}
