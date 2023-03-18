namespace BonusAccumulator.WordServices;

public interface ISearch
{
    IList<string> Query(string searchTerm);
}