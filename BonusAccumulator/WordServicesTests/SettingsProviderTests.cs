using BonusAccumulator;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class SettingsProviderTests
{
    private string _tempConfigPath = null!;

    [SetUp]
    public void SetUp()
    {
        _tempConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_{Guid.NewGuid()}.json");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempConfigPath))
        {
            File.Delete(_tempConfigPath);
        }
    }

    [Test]
    public void Constructor_DefaultPath_UsesAppSettingsJson()
    {
        SettingsProvider provider = new SettingsProvider();

        provider.Should().NotBeNull();
    }

    [Test]
    public void Constructor_CustomPath_UsesSpecifiedPath()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""TestKey"": ""TestValue""
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        provider.Should().NotBeNull();
    }

    [Test]
    public void GetSetting_ExistingKey_ReturnsValue()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""DictionaryPath"": ""test/dictionary.txt"",
            ""OtherSetting"": ""otherValue""
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("DictionaryPath");

        result.Should().Be("test/dictionary.txt");
    }

    [Test]
    public void GetSetting_NonExistingKey_ReturnsNull()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""ExistingKey"": ""SomeValue""
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("NonExistingKey");

        result.Should().BeNull();
    }

    [Test]
    public void GetSetting_EmptyKey_ReturnsNull()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""SomeKey"": ""SomeValue""
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("");

        result.Should().BeNull();
    }

    [Test]
    public void GetSetting_NullKey_ThrowsArgumentNullException()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""SomeKey"": ""SomeValue""
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        Action act = () => provider.GetSetting(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("key");
    }

    [Test]
    public void GetSetting_NestedConfiguration_ReturnsValue()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""Settings"": {
                ""DictionaryPath"": ""nested/dictionary.txt""
            }
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("Settings:DictionaryPath");

        result.Should().Be("nested/dictionary.txt");
    }

    [Test]
    public void GetSetting_NumericValue_ReturnsString()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""PortNumber"": 8080
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("PortNumber");

        result.Should().Be("8080");
    }

    [Test]
    public void GetSetting_BooleanValue_ReturnsString()
    {
        File.WriteAllText(_tempConfigPath, @"{
            ""FeatureEnabled"": true
        }");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("FeatureEnabled");

        result.Should().Be("True");
    }

    [Test]
    public void GetSetting_EmptyConfiguration_ReturnsNull()
    {
        File.WriteAllText(_tempConfigPath, "{}");

        SettingsProvider provider = new SettingsProvider(_tempConfigPath);

        string? result = provider.GetSetting("AnyKey");

        result.Should().BeNull();
    }

    [Test]
    public void GetSetting_NonExistentFile_ThrowsFileNotFoundException()
    {
        Action act = () => new SettingsProvider("non_existent_file.json");

        act.Should().Throw<FileNotFoundException>();
    }
}
