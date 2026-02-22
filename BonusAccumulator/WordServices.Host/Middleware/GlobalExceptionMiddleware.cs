using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WordServices.Host.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error on {Path}", context.Request.Path);
            await WriteProblemDetails(context, HttpStatusCode.BadRequest, "Invalid request", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation on {Path}", context.Request.Path);
            await WriteProblemDetails(context, HttpStatusCode.BadRequest, "Invalid operation", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
            await WriteProblemDetails(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred", "Please try again later.");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, HttpStatusCode statusCode,
        string title, string detail)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        ProblemDetails problem = new()
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
