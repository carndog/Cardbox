using System.ComponentModel.DataAnnotations;

namespace WordServices.Web.Models;

public record AnagramSearchRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(15)]
    [RegularExpression(@"^[A-Za-z?]+$")]
    public string Rack { get; set; } = string.Empty;

    public SearchMode Mode { get; set; } = SearchMode.Anagram;
}
