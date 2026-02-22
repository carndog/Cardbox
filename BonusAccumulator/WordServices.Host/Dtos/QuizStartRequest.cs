using System.ComponentModel.DataAnnotations;

namespace WordServices.Host.Dtos;

public record QuizStartRequest
{
    [Required]
    public QuizModeDto Mode { get; init; } = QuizModeDto.Session;
}
