namespace WordServices.Analytics;

public interface IAnalyticsService : IGetDeckStatsByCardbox, IGetDeckStatsByWordLength, IGetDueNow, IGetDueSoon, IGetHighestErrorRate, IGetMostWrong, IGetPainPerRecentMemory, IGetRegressions, IGetNotSeenForAges, IGetIntervalStats, IGetForgettingCurveStats, IGetBlindSpots, IGetPriorityItems
{
}
