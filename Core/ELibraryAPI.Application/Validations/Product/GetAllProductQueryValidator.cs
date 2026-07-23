using ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Product
{
    public sealed class GetAllProductQueryValidator : AbstractValidator<GetAllProductQueryRequest>
    {
        public GetAllProductQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Səhifə nömrəsi ən azı 1 olmalıdır.");

            RuleFor(x => x.Size)
                .GreaterThanOrEqualTo(1).WithMessage("Səhifə ölçüsü ən azı 1 olmalıdır.")
                .LessThanOrEqualTo(100).WithMessage("Səhifə ölçüsü 100-dən çox ola bilməz."); 

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum qiymət mənfi ola bilməz.")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Maksimum qiymət mənfi ola bilməz.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x => x.MaxPrice >= x.MinPrice)
                .WithMessage("Maksimum qiymət minimum qiymətdən böyük və ya bərabər olmalıdır.")
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.Search)
                .MinimumLength(2).WithMessage("Axtarış mətni ən azı 2 simvol uzunluğunda olmalıdır.")
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.SortBy)
                .Must(s => new[] { "PriceAsc", "PriceDesc", "Newest", "TopRated", null }.Contains(s))
                .WithMessage("Yanlış çeşidləmə seçimi.");
        }
    }
}
