using ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserSearchHistory;

public sealed class CreateUserSearchHistoryCommandValidator : AbstractValidator<CreateUserSearchHistoryCommandRequest>
{
    public CreateUserSearchHistoryCommandValidator()
    {
        RuleFor(x => x.SearchQuery)
            .NotEmpty().WithMessage("Axtarış sorğusu boş ola bilməz.")
            .MaximumLength(500).WithMessage("Axtarış sorğusu çox uzundur.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("İstifadəçi ID-si mütləqdir.");
    }
}