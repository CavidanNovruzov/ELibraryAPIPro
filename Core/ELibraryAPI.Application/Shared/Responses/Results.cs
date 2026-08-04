using System.Text.Json.Serialization;
using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Application.Responses;

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public List<string> Errors { get; init; } = new();
    public ErrorType ErrorType { get; init; }

    [JsonConstructor]
    protected Result(bool isSuccess, string? message, ErrorType errorType = ErrorType.None, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorType = errorType;
        Errors = errors ?? (!isSuccess && message is not null
            ? new List<string> { message }
            : new List<string>());
    }

    public static Result Success() => new(true, null);
    public static Result Success(string message) => new(true, message);

    public static Result Failure(string message, ErrorType errorType = ErrorType.BadRequest)
        => new(false, message, errorType);

    public static Result Failure(List<string> errors, string message = "Validasiya xətası baş verdi", ErrorType errorType = ErrorType.ValidationError)
        => new(false, message, errorType, errors);

    public static Result NotFound(string message = "Axtarılan məlumat tapılmadı") => Failure(message, ErrorType.NotFound);

    public static Result Conflict(string message)
        => Failure(message, ErrorType.Conflict);

    public static Result Forbidden(string message = "Bu əməliyyat üçün icazəniz yoxdur")
        => Failure(message, ErrorType.Forbidden);

    public static Result Unauthorized(string message = "İstifadəçi təsdiqlənməyib")
        => Failure(message, ErrorType.Unauthorized);
}

public class Result<T> : Result
{
    public T? Data { get; init; }

    [JsonConstructor]
    private Result(T? data, bool isSuccess, string? message, ErrorType errorType = ErrorType.None, List<string>? errors = null)
        : base(isSuccess, message, errorType, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(data, true, null);
    public static Result<T> Success(T data, string message) => new(data, true, message);

    public static new Result<T> Failure(string message, ErrorType errorType = ErrorType.BadRequest)
        => new(default, false, message, errorType);

    public static new Result<T> Failure(List<string> errors, string message = "Xəta baş verdi", ErrorType errorType = ErrorType.ValidationError)
        => new(default, false, message, errorType, errors);

    public static new Result<T> NotFound(string message = "Axtarılan məlumat tapılmadı") => Failure(message, ErrorType.NotFound);

    public static new Result<T> Conflict(string message)
           => Failure(message, ErrorType.Conflict);

    public static new Result<T> Forbidden(string message = "Bu əməliyyat üçün icazəniz yoxdur")
        => Failure(message, ErrorType.Forbidden);

    public static new Result<T> Unauthorized(string message = "İstifadəçi təsdiqlənməyib")
        => Failure(message, ErrorType.Unauthorized);
}