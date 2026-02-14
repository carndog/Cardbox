namespace WordServices.Analytics;

public interface IGetIntervalStats
{
    Task<IEnumerable<IntervalStats>> ExecuteAsync();
}
