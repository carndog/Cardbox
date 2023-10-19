namespace BonusAccumulator.WordServices;

public class SessionState
{
    public HashSet<string> SessionWords { get; } = new();
    
    public HashSet<string> AddedWords { get; } = new();
    
    public List<string> LastResult { get; } = new();

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
    
    public void AddLastWords()
    {
        AddedWords.UnionWith(LastResult);
        LastResult.Clear();
    }

    public string SaveAdded()
    {
        string name = DateTime.Now.ToString("s").Replace(":", "");
        string filePath = string.Format(Configuration.GetSetting("SessionOutputPath") ?? string.Empty, name);
        using (StreamWriter writer = new(filePath))
            writer.WriteLine(string.Join(Environment.NewLine, AddedWords));
        return filePath;
    }
}