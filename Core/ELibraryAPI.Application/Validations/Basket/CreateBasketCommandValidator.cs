using FluentValidation;
using ELibraryAPI.Application.Features.Commands.Basket.CreateBasket;

namespace ELibraryAPI.Application.Validations.Basket;

public sealed class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommandRequest>
{
    public CreateBasketCommandValidator()
    {
 
    }
}