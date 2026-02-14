namespace WordServices.Analytics;

public record PriorityItem(
    string Question,
    int Cardbox,
    int Difficulty,
    int Correct,
    int Incorrect,
    int Streak,
    DateTime DueAt,
    double Priority
);
