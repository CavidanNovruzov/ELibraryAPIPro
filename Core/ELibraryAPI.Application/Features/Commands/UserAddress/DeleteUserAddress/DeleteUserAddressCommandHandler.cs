using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.DeleteUserAddress;


public sealed class DeleteUserAddressCommandHandler : IRequestHandler<DeleteUserAddressCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserAddressCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) => (_unitOfWork, _currentUserService) = (unitOfWork, currentUserService);

    public async Task<Result> Handle(DeleteUserAddressCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserAddress, Guid>();

        var address = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (address == null) return Result.NotFound("Ünvan tapılmadı.");

        if (address.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu ünvana müdaxilə etmək üçün icazəniz yoxdur.");

        writeRepo.Remove(address);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Ünvan uğurla silindi.");
    }
}
