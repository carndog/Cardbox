using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public class QuestionHistory
{
    [Key]
    public string QuestionText { get; set; } = string.Empty;
    
    public int? TimeStamp { get; set; }
}
