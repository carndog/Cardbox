using System.ComponentModel.DataAnnotations;

namespace WordServices.Host.Dtos;

public record AnagramSearchRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(15)]
    [RegularExpression(@"^[A-Za-z?]+$")]
    public string Rack { get; init; } = string.Empty;

    public SearchMode Mode { get; init; } = SearchMode.Anagram;
}
