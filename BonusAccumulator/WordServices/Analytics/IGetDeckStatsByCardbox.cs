using WordServices.Analytics;

namespace WordServices.Analytics;

public interface IGetDeckStatsByCardbox
{
    Task<IEnumerable<CardboxStats>> ExecuteAsync();
}
