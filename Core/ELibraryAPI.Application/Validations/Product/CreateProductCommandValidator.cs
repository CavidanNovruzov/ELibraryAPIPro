using ELibraryAPI.Application.Features.Commands.Product.CreateProduct;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommandRequest>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.CoverTypeId)
            .NotEmpty().WithMessage("Üz qabığı növü boş ola bilməz.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Təsvir (açıqlama) boş ola bilməz.")
            .MaximumLength(1000).WithMessage("Təsvir 1000 simvoldan çox ola bilməz.");

        RuleFor(x => x.DiscountPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Endirimli qiymət 0-dan az ola bilməz.")
            .LessThanOrEqualTo(x => x.SalePrice).WithMessage("Endirimli qiymət satış qiymətindən baha ola bilməz.")
            .When(x => x.DiscountPrice != null);

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN nömrəsi boş ola bilməz.")
            .Length(10, 20).WithMessage("ISBN nömrəsinin uzunluğu 10 ilə 20 simvol arasında olmalıdır.");

        RuleFor(x => x.LanguageId)
            .NotEmpty().WithMessage("Dil seçimi mütləqdir.");

        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage("Səhifə sayı 0-dan böyük olmalıdır.");

        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage("Nəşriyyat mütləq seçilməlidir.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("Satış qiyməti 0-dan böyük olmalıdır.");

        RuleFor(x => x.SubCategoryId)
            .NotEmpty().WithMessage("Alt kateqoriya boş ola bilməz.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlıq boş ola bilməz.")
            .MaximumLength(250).WithMessage("Başlıq 250 simvoldan çox ola bilməz.");

        RuleFor(x => x.AuthorIds)
            .NotEmpty().WithMessage("Ən azı bir müəllif seçilməlidir.");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("Ən azı bir janr seçilməlidir.");

        RuleFor(x => x.PublicationYear)
            .GreaterThan(1000).WithMessage("Nəşr ili 1000-dən böyük olmalıdır.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage($"Nəşr ili {DateTime.UtcNow.Year}-dən böyük ola bilməz.");
    }
}