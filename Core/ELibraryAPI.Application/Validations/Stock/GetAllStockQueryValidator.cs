using ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Validations.Stock;

public sealed class GetAllStockQueryValidator : AbstractValidator<GetAllStockQueryRequest>
{
    public GetAllStockQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100 items per request.");
    }
}
