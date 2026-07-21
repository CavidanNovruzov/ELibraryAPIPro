using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateUserByAdmin;

public sealed class UpdateUserByAdminCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
    : IRequestHandler<UpdateUserByAdminCommandRequest, Result>
{
        public async Task<Result> Handle(UpdateUserByAdminCommandRequest request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
                return Result.NotFound("User not found.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;
            user.EmailConfirmed = request.EmailConfirmed;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result.Failure(errors, "An error occurred while updating the user.");
            }

            return Result.Success("User updated successfully by administrator.");
        }
    
}
