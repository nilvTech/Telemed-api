using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Telemed.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            // Handle 401 and 403 set by Authentication/Authorization middleware
            // This is the most important part for your current 401 issue
            if (!context.Response.HasStarted)
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Unauthorized access attempt to {Path}", context.Request.Path);

                    await HandleExceptionAsync(
                        context,
                        HttpStatusCode.Unauthorized,
                        "Unauthorized",
                        "Invalid or missing authentication token. Please provide a valid Bearer token.");
                    return;
                }

                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Forbidden access attempt to {Path}", context.Request.Path);

                    await HandleExceptionAsync(
                        context,
                        HttpStatusCode.Forbidden,
                        "Forbidden",
                        "You do not have permission to access this resource.");
                    return;
                }
            }
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Validation error: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadRequest,
                "Validation Error",
                ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized exception: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Not found: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                HttpStatusCode.NotFound,
                "Not Found",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception on {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string error,
        string message)
    {
        // Prevent writing if response has already started
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            error = error,
            message = message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.ToString(),
            correlationId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}