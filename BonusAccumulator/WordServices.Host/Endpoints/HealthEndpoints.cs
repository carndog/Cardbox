namespace WordServices.Host.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/api/health");

        app.MapGet("/api/health/detail", () =>
        {
            return Results.Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            });
        })
        .WithName("HealthDetail")
        .WithTags("Health");
    }
}
