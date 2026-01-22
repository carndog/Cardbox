namespace BonusAccumulator.WordServices;

public class SessionState : ISessionState
{
    private readonly ISettingsProvider _settingsProvider;
    
    public HashSet<string> SessionWords { get; } = new();
    
    public HashSet<string> AddedWords { get; } = new();
    
    public List<string> LastResult { get; } = new();

    public SessionState(ISettingsProvider settingsProvider)
    {
        _settingsProvider = settingsProvider;
    }

    public void Update(IList<string> words)
    {
        SessionWords.UnionWith(words);
        LastResult.Clear();
        LastResult.AddRange(words);
    }

    public void Add(IEnumerable<string> words)
    {
        AddedWords.UnionWith(words);
    }

    public string SaveAdded()
    {
        string name = DateTime.Now.ToString("s").Replace(":", "");
        string? outputPath = _settingsProvider.GetSetting("SessionOutputPath");
        if (string.IsNullOrEmpty(outputPath))
        {
            throw new InvalidOperationException("SessionOutputPath setting is not configured.");
        }
        string filePath = string.Format(outputPath, name);
        using StreamWriter writer = new(filePath);
        writer.Write(string.Join(Environment.NewLine, AddedWords));
        return filePath;
    }
}