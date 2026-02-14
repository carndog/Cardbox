using WordServices.Analytics;

namespace CardboxDataLayer.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IGetDeckStatsByCardbox _getDeckStatsByCardbox;
    private readonly IGetDeckStatsByWordLength _getDeckStatsByWordLength;
    private readonly IGetDueNow _getDueNow;
    private readonly IGetDueSoon _getDueSoon;
    private readonly IGetHighestErrorRate _getHighestErrorRate;
    private readonly IGetMostWrong _getMostWrong;
    private readonly IGetPainPerRecentMemory _getPainPerRecentMemory;
    private readonly IGetRegressions _getRegressions;
    private readonly IGetNotSeenForAges _getNotSeenForAges;
    private readonly IGetIntervalStats _getIntervalStats;
    private readonly IGetForgettingCurveStats _getForgettingCurveStats;
    private readonly IGetBlindSpots _getBlindSpots;
    private readonly IGetPriorityItems _getPriorityItems;

    public AnalyticsService(
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
    {
        _getDeckStatsByCardbox = getDeckStatsByCardbox;
        _getDeckStatsByWordLength = getDeckStatsByWordLength;
        _getDueNow = getDueNow;
        _getDueSoon = getDueSoon;
        _getHighestErrorRate = getHighestErrorRate;
        _getMostWrong = getMostWrong;
        _getPainPerRecentMemory = getPainPerRecentMemory;
        _getRegressions = getRegressions;
        _getNotSeenForAges = getNotSeenForAges;
        _getIntervalStats = getIntervalStats;
        _getForgettingCurveStats = getForgettingCurveStats;
        _getBlindSpots = getBlindSpots;
        _getPriorityItems = getPriorityItems;
    }

    Task<IEnumerable<CardboxStats>> IGetDeckStatsByCardbox.ExecuteAsync() => _getDeckStatsByCardbox.ExecuteAsync();

    Task<IEnumerable<WordLengthStats>> IGetDeckStatsByWordLength.ExecuteAsync() => _getDeckStatsByWordLength.ExecuteAsync();

    Task<IEnumerable<DueItem>> IGetDueNow.ExecuteAsync(int limit) => _getDueNow.ExecuteAsync(limit);

    Task<IEnumerable<DueItem>> IGetDueSoon.ExecuteAsync() => _getDueSoon.ExecuteAsync();

    Task<IEnumerable<ErrorRateStats>> IGetHighestErrorRate.ExecuteAsync(int limit) => _getHighestErrorRate.ExecuteAsync(limit);

    Task<IEnumerable<MostWrongStats>> IGetMostWrong.ExecuteAsync(int limit) => _getMostWrong.ExecuteAsync(limit);

    Task<IEnumerable<PainStats>> IGetPainPerRecentMemory.ExecuteAsync(int limit) => _getPainPerRecentMemory.ExecuteAsync(limit);

    Task<IEnumerable<RegressionStats>> IGetRegressions.ExecuteAsync(int limit) => _getRegressions.ExecuteAsync(limit);

    Task<IEnumerable<NotSeenForAgesStats>> IGetNotSeenForAges.ExecuteAsync(int limit) => _getNotSeenForAges.ExecuteAsync(limit);

    Task<IEnumerable<IntervalStats>> IGetIntervalStats.ExecuteAsync() => _getIntervalStats.ExecuteAsync();

    Task<IEnumerable<ForgettingCurveStats>> IGetForgettingCurveStats.ExecuteAsync() => _getForgettingCurveStats.ExecuteAsync();

    Task<IEnumerable<BlindSpotStats>> IGetBlindSpots.ExecuteAsync() => _getBlindSpots.ExecuteAsync();

    Task<IEnumerable<PriorityItem>> IGetPriorityItems.ExecuteAsync(int limit) => _getPriorityItems.ExecuteAsync(limit);

    public Task<IEnumerable<CardboxStats>> GetDeckStatsByCardboxAsync() => _getDeckStatsByCardbox.ExecuteAsync();

    public Task<IEnumerable<WordLengthStats>> GetDeckStatsByWordLengthAsync() => _getDeckStatsByWordLength.ExecuteAsync();

    public Task<IEnumerable<DueItem>> GetDueNowAsync(int limit = 200) => _getDueNow.ExecuteAsync(limit);

    public Task<IEnumerable<DueItem>> GetDueSoonAsync() => _getDueSoon.ExecuteAsync();

    public Task<IEnumerable<ErrorRateStats>> GetHighestErrorRateAsync(int limit = 100) => _getHighestErrorRate.ExecuteAsync(limit);

    public Task<IEnumerable<MostWrongStats>> GetMostWrongAsync(int limit = 100) => _getMostWrong.ExecuteAsync(limit);

    public Task<IEnumerable<PainStats>> GetPainPerRecentMemoryAsync(int limit = 100) => _getPainPerRecentMemory.ExecuteAsync(limit);

    public Task<IEnumerable<RegressionStats>> GetRegressionsAsync(int limit = 100) => _getRegressions.ExecuteAsync(limit);

    public Task<IEnumerable<NotSeenForAgesStats>> GetNotSeenForAgesAsync(int limit = 200) => _getNotSeenForAges.ExecuteAsync(limit);

    public Task<IEnumerable<IntervalStats>> GetIntervalStatsAsync() => _getIntervalStats.ExecuteAsync();

    public Task<IEnumerable<ForgettingCurveStats>> GetForgettingCurveStatsAsync() => _getForgettingCurveStats.ExecuteAsync();

    public Task<IEnumerable<BlindSpotStats>> GetBlindSpotsAsync() => _getBlindSpots.ExecuteAsync();

    public Task<IEnumerable<PriorityItem>> GetPriorityItemsAsync(int limit = 200) => _getPriorityItems.ExecuteAsync(limit);
}
