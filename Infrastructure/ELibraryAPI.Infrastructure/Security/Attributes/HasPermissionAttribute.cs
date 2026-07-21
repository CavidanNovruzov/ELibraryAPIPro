using Microsoft.AspNetCore.Authorization;

namespace ELibraryAPI.Infrastructure.Security.Attributes;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(permission)
    {
    }
}

