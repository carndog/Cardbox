using WordServices.Analytics;

namespace WordServices.Analytics;

public interface IGetHighestErrorRate
{
    Task<IEnumerable<ErrorRateStats>> ExecuteAsync(int limit = 100);
}
