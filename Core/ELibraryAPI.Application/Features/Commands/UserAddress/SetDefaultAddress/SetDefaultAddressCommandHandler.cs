using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.SetDefaultAddress;

public sealed class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetDefaultAddressCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetDefaultAddressCommandRequest request, CancellationToken ct)
    {
        var addressReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();

        var targetAddress = await addressReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (targetAddress == null)
            return Result.Failure("Ünvan tapılmadı.");

        if (targetAddress.IsDefault)
            return Result.Success("Bu ünvan artıq default olaraq təyin edilib.");

        var currentDefaults = await addressReadRepo
            .GetWhere(x => x.UserId == targetAddress.UserId && x.IsDefault && x.Id != targetAddress.Id, tracking: true)
            .ToListAsync(ct);

        foreach (var addr in currentDefaults)
        {
            addr.IsDefault = false;
        }


        targetAddress.IsDefault = true;

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Default ünvan uğurla yeniləndi.");
    }
}