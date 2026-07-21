using ELibraryAPI.Application.Features.Commands.Review.UpdateReview;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Review;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommandRequest>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Comment).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1,5);
        RuleFor(x => x.UserId).NotEmpty();
    }
}
