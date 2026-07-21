using ELibraryAPI.Application.Features.Commands.Product.UpdateProduct;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommandRequest>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CoverTypeId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.DiscountPrice).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.SalePrice).When(x => x.DiscountPrice != null);
        RuleFor(x => x.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(x => x.LanguageId).NotEmpty();
        RuleFor(x => x.PageCount).GreaterThan(0);
        RuleFor(x => x.PublisherId).NotEmpty();
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SubCategoryId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        RuleFor(x => x.PublicationYear)
        .GreaterThan(1000)
        .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
