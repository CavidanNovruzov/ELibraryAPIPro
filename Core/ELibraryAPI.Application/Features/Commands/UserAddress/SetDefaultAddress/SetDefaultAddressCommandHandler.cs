using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.SetDefaultAddress;

public sealed class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public SetDefaultAddressCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(SetDefaultAddressCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var addressReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();

        var targetAddress = await addressReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (targetAddress == null)
            return Result.NotFound("Ünvan tapılmadı.");

        if (targetAddress.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu ünvanı default etmək üçün icazəniz yoxdur.");

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
