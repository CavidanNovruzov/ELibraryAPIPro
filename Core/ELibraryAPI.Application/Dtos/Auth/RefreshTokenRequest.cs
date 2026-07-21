using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Auth;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = null!;
}
