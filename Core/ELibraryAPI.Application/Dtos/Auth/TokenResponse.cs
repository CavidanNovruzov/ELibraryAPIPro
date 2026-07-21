using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Auth;

public class TokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
