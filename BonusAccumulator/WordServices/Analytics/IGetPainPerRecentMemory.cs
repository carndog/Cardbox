namespace WordServices.Analytics;

public interface IGetPainPerRecentMemory
{
    Task<IEnumerable<PainStats>> ExecuteAsync(int limit = 100);
}
