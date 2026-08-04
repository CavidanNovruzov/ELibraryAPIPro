using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.DeleteOrder;

public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) => (_unitOfWork, _currentUserService) = (unitOfWork, currentUserService);

    public async Task<Result> Handle(DeleteOrderCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Failure("Sistemdə daxil edilməmisiniz.", ELibraryAPI.Domain.Enums.ErrorType.Unauthorized);

        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll()
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);

        if (order == null) return Result.NotFound("Sifariş tapılmadı..");

        if (order.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu sifarişi silmək üçün icazəniz yoxdur.");

        _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>().Remove(order);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Sifariş tamamilə silindi.");
    }
}
