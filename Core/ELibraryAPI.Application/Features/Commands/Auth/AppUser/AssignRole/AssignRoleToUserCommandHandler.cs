using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.AssignRole;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommandRequest, Result>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public AssignRoleToUserCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(AssignRoleToUserCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            return Result.Failure("User not found.");

        if (await _userManager.IsInRoleAsync(user, request.RoleName))
            return Result.Success("The user already has this role.");

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);

        if (result.Succeeded)
            return Result.Success($"{request.RoleName} is assigned to the user.");

        // Identity errors are combined and returned
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result.Failure(errors);
    }
}