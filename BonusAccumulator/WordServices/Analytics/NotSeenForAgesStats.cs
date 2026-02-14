namespace WordServices.Analytics;

public record NotSeenForAgesStats(
    string Question,
    int Cardbox,
    int Difficulty,
    DateTime LastCorrectAt,
    double DaysSinceLastCorrect
);
