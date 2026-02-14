namespace WordServices.Analytics;

public interface IGetNotSeenForAges
{
    Task<IEnumerable<NotSeenForAgesStats>> ExecuteAsync(int limit = 200);
}
