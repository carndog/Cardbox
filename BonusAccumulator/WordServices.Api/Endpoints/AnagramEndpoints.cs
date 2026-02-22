using WordServices.Api.Dtos;
using WordServices.Output;

namespace WordServices.Api.Endpoints;

public static class AnagramEndpoints
{
    public static void MapAnagramEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/anagrams").WithTags("Anagrams");

        group.MapPost("/search", (AnagramSearchRequest request,
            IWordService wordService,
            IWordOutputService outputService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Rack))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "Rack", ["Rack is required and must contain only letters."] }
                });
            }

            string rack = request.Rack.Trim().ToUpper();

            Answer answer = request.Mode switch
            {
                SearchMode.Build => wordService.Build(rack),
                SearchMode.Pattern => wordService.Pattern(rack),
                _ => wordService.Anagram(rack)
            };

            AnagramSearchResponse response = new()
            {
                Words = answer.Words.ToList().AsReadOnly(),
                Count = answer.Words.Count,
                FormattedResult = outputService.FormatWords(answer.Words),
                Rack = rack,
                Mode = request.Mode
            };

            return Results.Ok(response);
        })
        .WithName("SearchAnagrams")
        .Produces<AnagramSearchResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
