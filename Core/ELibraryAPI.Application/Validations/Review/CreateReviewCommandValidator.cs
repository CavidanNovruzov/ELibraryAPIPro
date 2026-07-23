using ELibraryAPI.Application.Features.Commands.Review.CreateReview;
using FluentValidation;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommandRequest>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Rəy boş ola bilməz.")
            .MaximumLength(500).WithMessage("Rəy 500 simvoldan az olmalıdır.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Qiymətləndirmə 1 ilə 5 arasında olmalıdır.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Məhsul qeyd edilməlidir.");

    }
}