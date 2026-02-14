namespace WordServices.Analytics;

public interface IGetDueNow
{
    Task<IEnumerable<DueItem>> ExecuteAsync(int limit = 200);
}
