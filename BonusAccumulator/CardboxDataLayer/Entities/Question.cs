using System.ComponentModel.DataAnnotations;

namespace CardboxDataLayer.Entities;

public class Question
{
    [Key]
    public string question { get; set; } = string.Empty;
    
    public int? correct { get; set; }
    
    public int? incorrect { get; set; }
    
    public int? streak { get; set; }
    
    public int? last_correct { get; set; }
    
    public int? difficulty { get; set; }
    
    public int? cardbox { get; set; }
    
    public int? next_scheduled { get; set; }
}
