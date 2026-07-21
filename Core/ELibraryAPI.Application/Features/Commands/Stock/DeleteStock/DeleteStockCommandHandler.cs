using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Stock.DeleteStock;

public sealed class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteStockCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Stock, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Stock, Guid>();

        var stock = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (stock == null)
        {
            return Result.Failure("Stock record not found.");
        }

        writeRepo.Remove(stock);


        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Stock record deleted successfully.");
    }
}