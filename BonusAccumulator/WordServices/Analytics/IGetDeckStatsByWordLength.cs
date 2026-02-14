namespace WordServices.Analytics;

public interface IGetDeckStatsByWordLength
{
    Task<IEnumerable<WordLengthStats>> ExecuteAsync();
}
