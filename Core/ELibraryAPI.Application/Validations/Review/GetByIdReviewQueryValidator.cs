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
            .NotEmpty().WithMessage("Rəy ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Düzgün Rəy ID-si təmin edilməlidir.");
    }
}
