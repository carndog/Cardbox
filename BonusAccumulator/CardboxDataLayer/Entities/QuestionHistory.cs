using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public class QuestionHistory
{
    [Key]
    public string question { get; set; } = string.Empty;
    
    public int? timeStamp { get; set; }
}
