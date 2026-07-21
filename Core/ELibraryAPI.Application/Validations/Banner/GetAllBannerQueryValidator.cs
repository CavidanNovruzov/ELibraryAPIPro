using ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Banner;

public sealed class GetAllBannerQueryValidator : AbstractValidator<GetAllBannerQueryRequest>
{
    public GetAllBannerQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə nömrəsi ən azı {ComparisonValue} olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}