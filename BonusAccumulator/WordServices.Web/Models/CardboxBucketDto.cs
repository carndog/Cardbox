namespace WordServices.Web.Models;

public record CardboxBucketDto
{
    public int Cardbox { get; init; }

    public int Items { get; init; }

    public int TotalReviews { get; init; }

    public double PercentCorrect { get; init; }
}
