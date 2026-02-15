namespace WordServices;

public class SessionState(ISettingsProvider settingsProvider) : ISessionState
{
    public HashSet<string> SessionWords { get; } = [];
    
    public HashSet<string> AddedWords { get; } = [];
    
    public List<string> LastResult { get; } = [];

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
        string? outputPath = settingsProvider.GetSetting("SessionOutputPath");
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