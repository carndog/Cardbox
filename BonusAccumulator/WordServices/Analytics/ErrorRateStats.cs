namespace WordServices.Analytics;

public record ErrorRateStats(
    string Question,
    int Correct,
    int Incorrect,
    int Attempts,
    double ErrorRate,
    int Streak,
    int Cardbox,
    int Difficulty
);
