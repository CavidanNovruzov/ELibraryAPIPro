
using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Application.Responses;

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public List<string>? Errors { get; init; }
    public ErrorType ErrorType { get; init; }

    protected Result(bool success, string? message, ErrorType errorType = ErrorType.None, List<string>? errors = null)
    {
        IsSuccess = success;
        Message = message;
        ErrorType = errorType;
        Errors = errors;
    }

    public static Result Success() => new(true, null);
    public static Result Success(string message) => new(true, message);

    public static Result Failure(string message, ErrorType errorType = ErrorType.BadRequest) 
        => new(false, message, errorType);

    public static Result Failure(List<string> errors, string message = "Validation failed", ErrorType errorType = ErrorType.ValidationError)
        => new(false, message, errorType, errors);

    public static Result NotFound(string message = "Resource not found") => Failure(message, ErrorType.NotFound);
}

public class Result<T> : Result
{
    public T? Data { get; init; }

    private Result(T? data, bool success, string? message, ErrorType errorType = ErrorType.None, List<string>? errors = null)
        : base(success, message, errorType, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(data, true, null);
    public static Result<T> Success(T data, string message) => new(data, true, message);

    public static new Result<T> Failure(string message, ErrorType errorType = ErrorType.BadRequest) 
        => new(default, false, message, errorType);

    public static new Result<T> Failure(List<string> errors, string message = "Error occurred", ErrorType errorType = ErrorType.ValidationError)
        => new(default, false, message, errorType, errors);

    public static new Result<T> NotFound(string message = "Resource not found") => Failure(message, ErrorType.NotFound);
}