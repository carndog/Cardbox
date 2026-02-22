using WordServices.Host.Dtos;

namespace WordServices.Host.Endpoints;

public static class QuizEndpoints
{
    public static void MapQuizEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/quiz").WithTags("Quiz");

        group.MapPost("/start", (QuizStartRequest request) =>
        {
            // TODO: Adapt RunQuiz for web — the CLI version uses Action<string>/Func<string?> callbacks.
            // For web, you'll need a stateful quiz session service that yields questions one at a time.
            // This stub returns a session ID so the API contract is established.

            string sessionId = Guid.NewGuid().ToString("N");

            QuizStartResponse response = new()
            {
                SessionId = sessionId,
                Message = "Quiz session created. Quiz endpoint requires adaptation from CLI callbacks to request/response pattern — see TODO."
            };

            return Results.Ok(response);
        })
        .WithName("StartQuiz")
        .Produces<QuizStartResponse>(StatusCodes.Status200OK);

        group.MapPost("/answer", (QuizAnswerRequest request) =>
        {
            // TODO: Look up session by request.SessionId, evaluate answer, return result.
            // Requires a scoped/session-based quiz state service to replace CLI I/O callbacks.

            QuizAnswerResponse response = new()
            {
                IsCorrect = false,
                Feedback = "Quiz answer evaluation not yet implemented — requires CLI-to-web session adapter. See TODO.",
                CorrectAnswers = []
            };

            return Results.Ok(response);
        })
        .WithName("AnswerQuiz")
        .Produces<QuizAnswerResponse>(StatusCodes.Status200OK);
    }
}
