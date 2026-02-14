namespace WordServices.Analytics;

public record BlindSpotStats(
    int Difficulty,
    int Length,
    int Items,
    double PercentCorrect,
    int Reviews
);
