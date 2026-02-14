namespace WordServices.Analytics;

public record IntervalStats(
    int Cardbox,
    int Items,
    double AverageIntervalDays,
    double MinimumIntervalDays,
    double MaximumIntervalDays
);
