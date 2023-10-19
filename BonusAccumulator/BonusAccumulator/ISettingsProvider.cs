namespace BonusAccumulator;

public interface ISettingsProvider
{
    string? GetSetting(string key);
}