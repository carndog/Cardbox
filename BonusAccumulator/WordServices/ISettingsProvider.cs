namespace WordServices;

public interface ISettingsProvider
{
    string? GetSetting(string key);
}
