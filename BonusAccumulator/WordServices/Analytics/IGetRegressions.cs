namespace WordServices.Analytics;

public interface IGetRegressions
{
    Task<IEnumerable<RegressionStats>> ExecuteAsync(int limit = 100);
}
