

namespace ELibraryAPI.Domain.Enums;

public enum ErrorType
{
    None,
    BadRequest,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    ValidationError,
    ServerError
}
