using ELibraryAPI.Application.Features.Commands.Review.CreateReview;
using FluentValidation;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommandRequest>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment cannot be empty.")
            .MaximumLength(500).WithMessage("Comment must be less than 500 characters.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product must be specified.");

    }
}