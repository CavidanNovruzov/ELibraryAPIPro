using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.UpdateUserAddress;

public sealed class UpdateUserAddressCommandHandler : IRequestHandler<UpdateUserAddressCommandRequest, Result<UpdateUserAddressCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateUserAddressCommandResponse>> Handle(UpdateUserAddressCommandRequest request, CancellationToken ct)
    {
        var addressReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();

        var address = await addressReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (address == null) return Result<UpdateUserAddressCommandResponse>.Failure("Address not found.");

        if (request.IsDefault && !address.IsDefault)
        {
            var otherDefaults = await addressReadRepo
                .GetWhere(x => x.UserId == address.UserId && x.IsDefault && x.Id != address.Id, tracking: true)
                .ToListAsync(ct);

            foreach (var ad in otherDefaults) ad.IsDefault = false;
        }

        _mapper.Map(request, address);
        address.AddressLine = request.AddressLine.Trim();

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateUserAddressCommandResponse>.Success(new(address.Id), "User address updated.");
    }
}