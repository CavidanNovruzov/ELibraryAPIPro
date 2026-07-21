using ELibraryAPI.Application.Features.Queries.Review.GetAllReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Validations.Review;

public sealed class GetAllReviewQueryValidator : AbstractValidator<GetAllReviewQueryRequest>
{
    public GetAllReviewQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
    }
}
