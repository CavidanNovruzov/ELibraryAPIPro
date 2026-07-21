using ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Validations.Review;

public sealed class GetByIdReviewQueryValidator : AbstractValidator<GetByIdReviewQueryRequest>
{
    public GetByIdReviewQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Review ID is required.")
            .NotEqual(Guid.Empty).WithMessage("A valid Review ID must be provided.");
    }
}
