using ELibraryAPI.Application.Features.Commands.Product.UpdateProduct;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommandRequest>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.CoverTypeId).NotEmpty().WithMessage("Cover növü ID-si boş ola bilməz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("təsviri boş ola bilməz.");
        RuleFor(x => x.DiscountPrice).GreaterThanOrEqualTo(0).WithMessage("Discount qiyməti {ComparisonValue}-dan böyük olmalıdır.").LessThanOrEqualTo(x => x.SalePrice).When(x => x.DiscountPrice != null);
        RuleFor(x => x.ISBN).NotEmpty().WithMessage("ISBN boş ola bilməz.").MaximumLength(20).WithMessage("ISBN maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.LanguageId).NotEmpty().WithMessage("Dil ID-si boş ola bilməz.");
        RuleFor(x => x.PageCount).GreaterThan(0).WithMessage("Page sayı {ComparisonValue}-dan böyük olmalıdır.");
        RuleFor(x => x.PublisherId).NotEmpty().WithMessage("Nəşriyyat ID-si boş ola bilməz.");
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0).WithMessage("Sale qiyməti {ComparisonValue}-dan böyük olmalıdır.");
        RuleFor(x => x.SubCategoryId).NotEmpty().WithMessage("Sub Kateqoriya ID-si boş ola bilməz.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("başlığı boş ola bilməz.").MaximumLength(250).WithMessage("başlığı maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.PublicationYear)
        .GreaterThan(1000).WithMessage("Publication Year {ComparisonValue}-dan böyük olmalıdır.")
        .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
