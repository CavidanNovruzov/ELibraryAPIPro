using ELibraryAPI.Application.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace ELibraryAPI.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error");
            await WriteResponse(context, 400, "Validation failed",
                errors: ex.Errors.Select(e => e.ErrorMessage).ToList());
        }
        catch (NotFoundException ex)     
        {
            _logger.LogWarning(ex, "Resource not found");
            await WriteResponse(context, 404, ex.Message);
        }
        catch (UnauthorizedAccessException ex) 
        {
            _logger.LogWarning(ex, "Unauthorized");
            await WriteResponse(context, 401, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, 500, "Internal Server Error",
                traceId: context.TraceIdentifier);
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        int statusCode,
        string title,
        List<string>? errors = null,
        string? traceId = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new Dictionary<string, object?>
        {
            ["type"] = $"https://httpstatuses.com/{statusCode}",
            ["title"] = title,
            ["status"] = statusCode,
        };

        if (errors is { Count: > 0 }) problem["errors"] = errors;
        if (traceId is not null) problem["traceId"] = traceId;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}

