using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.Review.ApproveReview;

public sealed class ApproveReviewCommandValidator : AbstractValidator<ApproveReviewCommandRequest>
{
    public ApproveReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Rəy ID-si mütləqdir.");
    }
}