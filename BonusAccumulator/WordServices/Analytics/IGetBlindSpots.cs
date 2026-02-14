using WordServices.Analytics;

namespace WordServices.Analytics;

public interface IGetBlindSpots
{
    Task<IEnumerable<BlindSpotStats>> ExecuteAsync();
}
