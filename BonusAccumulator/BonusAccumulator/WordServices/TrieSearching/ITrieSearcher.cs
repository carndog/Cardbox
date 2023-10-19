namespace BonusAccumulator.WordServices.TrieSearching;

public interface ITrieSearcher
{
    IList<string> Query(string searchTerm, Func<IEnumerable<string>, IEnumerable<string>> wordFilter);
}