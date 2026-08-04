using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;

public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommandRequest, Result<UpdateProfileCommandResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;
    private readonly ICacheService _cacheService;

    public UpdateProfileCommandHandler(
        UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _cacheService = cacheService;
    }

    public async Task<Result<UpdateProfileCommandResponse>> Handle(
        UpdateProfileCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<UpdateProfileCommandResponse>.Failure("İstifadəçi tapılmadı.");

        if (!string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userManager.FindByNameAsync(request.UserName);
            if (existing != null)
                return Result<UpdateProfileCommandResponse>.Failure("Bu istifadəçi adı artıq mövcuddur.");
        }

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.UserName = request.UserName.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Result<UpdateProfileCommandResponse>.Failure(
                result.Errors.Select(e => e.Description).ToList());

        string profileCacheKey = $"user:profile:{user.Id}";
        string detailCacheKey = $"user:detail:{user.Id}";

        await _cacheService.RemoveAsync(profileCacheKey, ct);
        await _cacheService.RemoveAsync(detailCacheKey, ct);

        return Result<UpdateProfileCommandResponse>.Success(
            new UpdateProfileCommandResponse(user.Id, $"{user.FirstName} {user.LastName}"),
            "Əməliyyat uğurla tamamlandı.");
    }
}