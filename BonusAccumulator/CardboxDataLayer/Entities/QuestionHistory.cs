using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public record QuestionHistory
{
    [Key]
    public string QuestionText { get; init; } = string.Empty;
    
    public int? TimeStamp { get; init; }
}
