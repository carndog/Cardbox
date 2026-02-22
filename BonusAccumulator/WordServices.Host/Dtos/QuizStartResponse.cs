namespace WordServices.Host.Dtos;

public record QuizStartResponse
{
    public string SessionId { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}
