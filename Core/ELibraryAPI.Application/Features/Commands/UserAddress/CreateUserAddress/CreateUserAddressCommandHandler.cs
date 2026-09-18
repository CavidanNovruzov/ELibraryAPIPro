using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class CreateUserAddressCommandHandler : IRequestHandler<CreateUserAddressCommandRequest, Result<CreateUserAddressCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateUserAddressCommandResponse>> Handle(CreateUserAddressCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (!userId.HasValue)
            return Result<CreateUserAddressCommandResponse>.Failure("Sistemə daxil olunmamışdır.", ErrorType.Unauthorized);

        var addressReadRepo = _unitOfWork.ReadRepository<ELibraryAPI.Domain.Entities.Concrete.UserAddress, Guid>();
        var addressWriteRepo = _unitOfWork.WriteRepository<ELibraryAPI.Domain.Entities.Concrete.UserAddress, Guid>();

        if (request.IsDefault)
        {
            var oldDefaultAddresses = await addressReadRepo
                .GetWhere(x => x.UserId == userId && x.IsDefault, tracking: true) 
                .ToListAsync(ct);

            foreach (var oldAddr in oldDefaultAddresses)
                oldAddr.IsDefault = false;
        }

        var userAddress = _mapper.Map<ELibraryAPI.Domain.Entities.Concrete.UserAddress>(request);
        userAddress.AddressLine = request.AddressLine.Trim();

        userAddress.UserId = userId.Value;

        await addressWriteRepo.AddAsync(userAddress, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateUserAddressCommandResponse>.Success(new(userAddress.Id), "İstifadəçi ünvanı yaradıldı.");
    }
}