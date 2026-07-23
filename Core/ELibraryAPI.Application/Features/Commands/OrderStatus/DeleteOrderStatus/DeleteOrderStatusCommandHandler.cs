using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.DeleteOrderStatus;

public sealed class DeleteOrderStatusCommandHandler : IRequestHandler<DeleteOrderStatusCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderStatusCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.OrderStatus, Guid>();

        var status = await readRepo.GetAll(tracking: true)
            .Include(x => x.Orders)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

        if (status == null)
        {
            return Result.Failure("Sifariş statusu tapılmadı.");
        }

        if (status.Orders.Any())
        {
            return Result.Failure("Bu status mövcud sifarişlərdə istifadə olunur və silinə bilməz.");
        }

        writeRepo.Remove(status);
        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success("Sifariş statusu uğurla silindi.")
            : Result.Failure("Sifariş statusu silinərkən xəta baş verdi.");
    }
}