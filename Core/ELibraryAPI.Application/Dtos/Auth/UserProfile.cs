using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Auth;

public class UserProfile
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
}