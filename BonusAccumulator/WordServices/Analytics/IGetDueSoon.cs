namespace WordServices.Analytics;

public interface IGetDueSoon
{
    Task<IEnumerable<DueItem>> ExecuteAsync();
}
