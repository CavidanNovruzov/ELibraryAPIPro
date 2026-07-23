
namespace ELibraryAPI.Application.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException() : base("İstifadəçi yaradılması uğursuz oldu.")
        {
            
        }
        public UserCreateFailedException(string message) : base(message)
        {
        }

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
