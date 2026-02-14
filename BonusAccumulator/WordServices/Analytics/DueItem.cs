namespace WordServices.Analytics;

public record DueItem(
    string Question,
    int Cardbox,
    int Difficulty,
    DateTime DueAt
);
