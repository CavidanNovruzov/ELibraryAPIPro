using ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserSearchHistory;

public sealed class DeleteUserSearchHistoryCommandValidator : AbstractValidator<DeleteUserSearchHistoryCommandRequest>
{
    public DeleteUserSearchHistoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
