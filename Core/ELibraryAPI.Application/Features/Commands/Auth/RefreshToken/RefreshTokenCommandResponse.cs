using ELibraryAPI.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.RefreshToken;

public record RefreshTokenCommandResponse(TokenResponse Token);

