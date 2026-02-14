namespace WordServices.Analytics;

public record IntervalStats(
    int Cardbox,
    int Items,
    double AvgIntervalDays,
    double MinIntervalDays,
    double MaxIntervalDays
);
