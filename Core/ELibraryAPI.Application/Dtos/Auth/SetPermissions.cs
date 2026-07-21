
namespace ELibraryAPI.Application.Dtos.Auth;

public record SetPermissionsDto(Guid TargetId, List<Guid> PermissionIds);
