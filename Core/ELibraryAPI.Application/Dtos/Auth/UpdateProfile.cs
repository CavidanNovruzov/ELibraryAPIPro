using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Auth;

public class UpdateProfile
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
