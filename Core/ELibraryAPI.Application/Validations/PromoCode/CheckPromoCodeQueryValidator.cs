using ELibraryAPI.Application.Features.Queries.PromoCode.CheckPromoCode;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Validations.PromoCode;

public sealed class CheckPromoCodeQueryValidator : AbstractValidator<CheckPromoCodeQueryRequest>
{
    public CheckPromoCodeQueryValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Promo kod boş ola bilməz.")
            .MinimumLength(3).WithMessage("Promo kod ən azı 3 simvol olmalıdır.")
            .MaximumLength(20).WithMessage("Promo kod 20 simvoldan çox ola bilməz.")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Promo kodda yalnız hərf, rəqəm, tire və alt xətt ola bilər.");
    }
}
