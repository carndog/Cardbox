using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public record Question
{
    [Key]
    public string QuestionText { get; init; } = string.Empty;
    
    public int? Correct { get; init; }
    
    public int? Incorrect { get; init; }
    
    public int? Streak { get; init; }
    
    public int? LastCorrect { get; init; }
    
    public int? Difficulty { get; init; }
    
    public int? Cardbox { get; init; }
    
    public int? NextScheduled { get; init; }
}
