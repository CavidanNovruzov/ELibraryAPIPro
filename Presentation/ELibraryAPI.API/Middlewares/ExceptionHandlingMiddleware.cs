using ELibraryAPI.Application.Exceptions;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Domain.Enums;
using FluentValidation;

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
            var result = Result.Failure(
                ex.Errors.Select(e => e.ErrorMessage).ToList(),
                "Validasiya xətası baş verdi.",
                ErrorType.ValidationError);
            await WriteResponse(context, 422, result);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Resource not found");
            var result = Result.Failure("Axtarılan məlumat tapılmadı.", ErrorType.NotFound);
            await WriteResponse(context, 404, result);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning("Unauthorized access attempt");
            var result = Result.Failure("Bu əməliyyat üçün icazəniz yoxdur.", ErrorType.Forbidden);
            await WriteResponse(context, 403, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);
            var result = Result.Failure("Sistemdə gözlənilməz xəta baş verdi.", ErrorType.ServerError);
            await WriteResponse(context, 500, result);
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, Result result)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(result);
    }
}
