using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Auth;

public class ConfirmEmailRequest
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
}
