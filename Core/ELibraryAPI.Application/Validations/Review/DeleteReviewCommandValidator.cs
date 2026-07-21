using ELibraryAPI.Application.Features.Commands.Review.DeleteReview;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Review;

public sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommandRequest>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
