namespace WordServices.Analytics;

public record PainStats(
    string Question,
    int Incorrect,
    int Correct,
    int Streak,
    int Cardbox,
    int Difficulty
);
