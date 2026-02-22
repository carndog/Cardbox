using CardboxDataLayer.Repositories;
using WordServices.Analytics;
using WordServices.Api.Dtos;

namespace WordServices.Api.Endpoints;

public static class CardboxEndpoints
{
    public static void MapCardboxEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/cardbox").WithTags("Cardbox");

        group.MapGet("/stats", async (IQuestionRepository questionRepository,
            IAnalyticsService analyticsService) =>
        {
            int totalCount = await questionRepository.GetTotalCountAsync();
            double averageDifficulty = await questionRepository.GetAverageDifficultyAsync();
            IGetDeckStatsByCardbox cardboxQuery = analyticsService;
            IEnumerable<CardboxStats> deckStats = await cardboxQuery.ExecuteAsync();

            List<CardboxBucketDto> buckets = deckStats.Select(s => new CardboxBucketDto
            {
                Cardbox = s.Cardbox,
                Items = s.Items,
                TotalReviews = s.TotalReviews,
                PercentCorrect = s.PercentCorrect
            }).ToList();

            CardboxStatsResponse response = new()
            {
                TotalQuestions = totalCount,
                AverageDifficulty = averageDifficulty,
                Buckets = buckets.AsReadOnly()
            };

            return Results.Ok(response);
        })
        .WithName("GetCardboxStats")
        .Produces<CardboxStatsResponse>(StatusCodes.Status200OK)
        ;
    }
}
