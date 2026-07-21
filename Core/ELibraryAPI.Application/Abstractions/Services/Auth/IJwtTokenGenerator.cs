using ELibraryAPI.Application.Dtos.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;


namespace ELibraryAPI.Application.Abstractions.Services.Auth;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(AppUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
}
