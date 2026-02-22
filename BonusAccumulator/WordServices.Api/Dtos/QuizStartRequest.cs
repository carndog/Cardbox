using System.ComponentModel.DataAnnotations;

namespace WordServices.Api.Dtos;

public record QuizStartRequest
{
    [Required]
    public QuizModeDto Mode { get; init; } = QuizModeDto.Session;
}
