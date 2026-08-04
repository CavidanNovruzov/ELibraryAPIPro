using ELibraryAPI.Application.Features.Commands.InventoryMovement.AdjustStock;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.InventoryMovement;

public sealed class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommandRequest>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Filial ID-si boş ola bilməz.");
        RuleFor(x => x.QuantityDelta).NotEqual(0).WithMessage("Dəyişiklik miqdarı 0 ola bilməz.");
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500)
            .WithMessage("Korreksiya səbəbi qeyd edilməlidir (maksimum {MaxLength} simvol).");
        RuleFor(x => x.Type).IsInEnum().WithMessage("Hərəkət növü düzgün deyil.");
    }
}
