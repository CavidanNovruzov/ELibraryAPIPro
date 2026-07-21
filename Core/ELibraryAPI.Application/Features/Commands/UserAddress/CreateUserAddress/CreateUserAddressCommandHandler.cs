using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;

public sealed class CreateUserAddressCommandHandler : IRequestHandler<CreateUserAddressCommandRequest, Result<CreateUserAddressCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateUserAddressCommandResponse>> Handle(CreateUserAddressCommandRequest request, CancellationToken ct)
    {
        var addressReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();
        var addressWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserAddress, Guid>();
        var userReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.AppUser, Guid>();

        if (!await userReadRepo.ExistsAsync(x => x.Id == request.UserId, false, ct))
            return Result<CreateUserAddressCommandResponse>.Failure("User not found.");

        if (request.IsDefault)
        {
            var oldDefaultAddresses = await addressReadRepo
                .GetWhere(x => x.UserId == request.UserId && x.IsDefault, tracking: true)
                .ToListAsync(ct);

            foreach (var oldAddr in oldDefaultAddresses)
                oldAddr.IsDefault = false;
        }

        var userAddress = _mapper.Map<Domain.Entities.Concrete.UserAddress>(request);
        userAddress.AddressLine = request.AddressLine.Trim();

        await addressWriteRepo.AddAsync(userAddress, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateUserAddressCommandResponse>.Success(new(userAddress.Id), "User address created.");
    }
}