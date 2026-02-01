using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public class Question
{
    [Key]
    public string QuestionText { get; set; } = string.Empty;
    
    public int? Correct { get; set; }
    
    public int? Incorrect { get; set; }
    
    public int? Streak { get; set; }
    
    public int? LastCorrect { get; set; }
    
    public int? Difficulty { get; set; }
    
    public int? Cardbox { get; set; }
    
    public int? NextScheduled { get; set; }
}
