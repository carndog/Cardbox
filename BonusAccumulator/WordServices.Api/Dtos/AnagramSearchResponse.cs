namespace WordServices.Api.Dtos;

public record AnagramSearchResponse
{
    public IReadOnlyList<string> Words { get; init; } = [];

    public int Count { get; init; }

    public string FormattedResult { get; init; } = string.Empty;

    public string Rack { get; init; } = string.Empty;

    public SearchMode Mode { get; init; }
}
