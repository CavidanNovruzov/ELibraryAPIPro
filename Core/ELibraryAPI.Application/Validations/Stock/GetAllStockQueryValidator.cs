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
            .WithMessage("Səhifə nömrəsi ən azı 1 olmalıdır.");

        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə ölçüsü ən azı 1 olmalıdır.")
            .LessThanOrEqualTo(100)
            .WithMessage("Səhifə ölçüsü bir sorğu üçün 100 elementdən çox ola bilməz.");
    }
}
