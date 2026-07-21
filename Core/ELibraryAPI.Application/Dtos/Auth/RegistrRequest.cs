
namespace ELibraryAPI.Application.Dtos.Auth
{
    public class RegistrRequest
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
