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
            .GreaterThanOrEqualTo(1).WithMessage("Səhifə nömrəsi ən azı 1 olmalıdır.");

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1).WithMessage("Səhifə ölçüsü ən azı 1 olmalıdır.")
            .LessThanOrEqualTo(100).WithMessage("Səhifə ölçüsü 100-dən çox ola bilməz.");
    }
}
