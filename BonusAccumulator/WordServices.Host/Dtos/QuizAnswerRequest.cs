using System.ComponentModel.DataAnnotations;

namespace WordServices.Host.Dtos;

public record QuizAnswerRequest
{
    [Required]
    public string SessionId { get; init; } = string.Empty;

    [Required]
    public string Answer { get; init; } = string.Empty;
}
