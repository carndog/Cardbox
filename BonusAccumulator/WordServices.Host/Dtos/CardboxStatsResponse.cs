namespace WordServices.Host.Dtos;

public record CardboxStatsResponse
{
    public int TotalQuestions { get; init; }

    public double AverageDifficulty { get; init; }

    public IReadOnlyList<CardboxBucketDto> Buckets { get; init; } = [];
}
