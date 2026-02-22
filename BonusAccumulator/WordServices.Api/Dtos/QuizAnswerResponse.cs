namespace WordServices.Api.Dtos;

public record QuizAnswerResponse
{
    public bool IsCorrect { get; init; }

    public string Feedback { get; init; } = string.Empty;

    public IReadOnlyList<string> CorrectAnswers { get; init; } = [];
}
