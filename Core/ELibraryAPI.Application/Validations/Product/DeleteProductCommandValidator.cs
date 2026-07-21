using ELibraryAPI.Application.Features.Commands.Product.DeleteProduct;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommandRequest>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
