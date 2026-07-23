using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.DeleteOrder;

public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteOrderCommandRequest request, CancellationToken ct)
    {
        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll()
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);

        if (order == null) return Result.Failure("Sifariş tapılmadı..");

        _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>().Remove(order);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Sifariş tamamilə silindi.");
    }
}