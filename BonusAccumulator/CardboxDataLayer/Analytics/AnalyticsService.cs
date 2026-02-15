using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class AnalyticsService(
    IGetDeckStatsByCardbox getDeckStatsByCardbox,
    IGetDeckStatsByWordLength getDeckStatsByWordLength,
    IGetDueNow getDueNow,
    IGetDueSoon getDueSoon,
    IGetHighestErrorRate getHighestErrorRate,
    IGetMostWrong getMostWrong,
    IGetPainPerRecentMemory getPainPerRecentMemory,
    IGetRegressions getRegressions,
    IGetNotSeenForAges getNotSeenForAges,
    IGetIntervalStats getIntervalStats,
    IGetForgettingCurveStats getForgettingCurveStats,
    IGetBlindSpots getBlindSpots,
    IGetPriorityItems getPriorityItems)
    : IAnalyticsService
{
    Task<IEnumerable<CardboxStats>> IGetDeckStatsByCardbox.ExecuteAsync() => getDeckStatsByCardbox.ExecuteAsync();

    Task<IEnumerable<WordLengthStats>> IGetDeckStatsByWordLength.ExecuteAsync() => getDeckStatsByWordLength.ExecuteAsync();

    Task<IEnumerable<DueItem>> IGetDueNow.ExecuteAsync(int limit) => getDueNow.ExecuteAsync(limit);

    Task<IEnumerable<DueItem>> IGetDueSoon.ExecuteAsync() => getDueSoon.ExecuteAsync();

    Task<IEnumerable<ErrorRateStats>> IGetHighestErrorRate.ExecuteAsync(int limit) => getHighestErrorRate.ExecuteAsync(limit);

    Task<IEnumerable<MostWrongStats>> IGetMostWrong.ExecuteAsync(int limit) => getMostWrong.ExecuteAsync(limit);

    Task<IEnumerable<PainStats>> IGetPainPerRecentMemory.ExecuteAsync(int limit) => getPainPerRecentMemory.ExecuteAsync(limit);

    Task<IEnumerable<RegressionStats>> IGetRegressions.ExecuteAsync(int limit) => getRegressions.ExecuteAsync(limit);

    Task<IEnumerable<NotSeenForAgesStats>> IGetNotSeenForAges.ExecuteAsync(int limit) => getNotSeenForAges.ExecuteAsync(limit);

    Task<IEnumerable<IntervalStats>> IGetIntervalStats.ExecuteAsync() => getIntervalStats.ExecuteAsync();

    Task<IEnumerable<ForgettingCurveStats>> IGetForgettingCurveStats.ExecuteAsync() => getForgettingCurveStats.ExecuteAsync();

    Task<IEnumerable<BlindSpotStats>> IGetBlindSpots.ExecuteAsync() => getBlindSpots.ExecuteAsync();

    Task<IEnumerable<PriorityItem>> IGetPriorityItems.ExecuteAsync(int limit) => getPriorityItems.ExecuteAsync(limit);
}
