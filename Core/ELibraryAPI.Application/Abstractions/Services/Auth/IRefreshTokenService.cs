using ELibraryAPI.Domain.Entities.Concrete.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Abstractions.Services.Auth;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(AppUser user,CancellationToken ct=default);
    Task<AppUser?> ValidateAndConsumeAsync(string token,CancellationToken ct=default);
}
