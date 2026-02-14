namespace WordServices.Analytics;

public interface IGetPriorityItems
{
    Task<IEnumerable<PriorityItem>> ExecuteAsync(int limit = 200);
}
