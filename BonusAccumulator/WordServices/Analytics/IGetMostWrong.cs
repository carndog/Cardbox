namespace WordServices.Analytics;

public interface IGetMostWrong
{
    Task<IEnumerable<MostWrongStats>> ExecuteAsync(int limit = 100);
}
