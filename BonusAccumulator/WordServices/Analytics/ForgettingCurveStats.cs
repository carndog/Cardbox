namespace WordServices.Analytics;

public record ForgettingCurveStats(
    string AgeBucket,
    int Items,
    double PercentCorrect
);
