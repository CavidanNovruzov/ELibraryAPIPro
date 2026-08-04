using ELibraryAPI.Application.Responses;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.InventoryMovement.AdjustStock;

/// <summary>
/// Admin/anbar menecerinin əl ilə stok korreksiyası etməsi üçün (inventarizasiya
/// uyğunsuzluğu, zədələnmiş məhsul, itki və s.). Sifariş zamanı yaradılan
/// "Sale" tipli InventoryMovement-lərdən fərqli olaraq, bu, sistemdə İNDİYƏ
/// QƏDƏR olmayan yeganə "InventoryMovement yaratma yolu"dur — əvvəllər
/// InventoryMovement yalnız CreateOrder/CancelOrder handler-lərinin daxili
/// yan effekti kimi mövcud idi, xaricdən heç bir giriş nöqtəsi yox idi.
///
/// Quantity işarəsi: müsbət = anbara əlavə (tapılan məhsul), mənfi = çıxma (itki/zədə).
/// </summary>
public sealed record AdjustStockCommandRequest(
    Guid ProductId,
    Guid BranchId,
    int QuantityDelta,
    InventoryMovementType Type,
    string Reason
) : IRequest<Result<AdjustStockCommandResponse>>;
