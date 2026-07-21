using ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserSearchHistory;

public sealed class CreateUserSearchHistoryCommandValidator : AbstractValidator<CreateUserSearchHistoryCommandRequest>
{
    public CreateUserSearchHistoryCommandValidator()
    {
        RuleFor(x => x.SearchQuery)
            .NotEmpty().WithMessage("Search query cannot be empty.")
            .MaximumLength(500).WithMessage("Search query is too long.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}