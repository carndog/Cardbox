namespace BonusAccumulator.WordServices;

public interface ISessionState
{
    HashSet<string> SessionWords { get; }
    HashSet<string> AddedWords { get; }
    List<string> LastResult { get; }
    void Update(IList<string> words);
    void Add(IEnumerable<string> words);
    string SaveAdded();
}