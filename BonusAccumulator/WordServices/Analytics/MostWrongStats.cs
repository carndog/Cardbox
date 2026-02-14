namespace WordServices.Analytics;

public record MostWrongStats(
    string Question,
    int Incorrect,
    int Correct,
    int Attempts,
    int Streak,
    int Cardbox,
    int Difficulty
);
