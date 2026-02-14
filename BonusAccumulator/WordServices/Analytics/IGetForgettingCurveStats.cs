using WordServices.Analytics;

namespace WordServices.Analytics;

public interface IGetForgettingCurveStats
{
    Task<IEnumerable<ForgettingCurveStats>> ExecuteAsync();
}
