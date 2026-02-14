namespace WordServices.Analytics;

public record RegressionStats(
    string Question,
    int Correct,
    int Incorrect,
    int Streak,
    int Cardbox,
    int Difficulty,
    DateTime LastCorrectAt
);
