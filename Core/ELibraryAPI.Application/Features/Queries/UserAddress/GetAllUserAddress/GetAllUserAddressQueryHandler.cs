using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.UserAddress.GetAllUserAddress;

public sealed class GetAllUserAddressQueryHandler : IRequestHandler<GetAllUserAddressQueryRequest, Result<GetAllUserAddressQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetAllUserAddressQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetAllUserAddressQueryResponse>> Handle(GetAllUserAddressQueryRequest request, CancellationToken cancellationToken)
    {
        Guid currentUserId = Guid.Parse(_currentUserService.UserId.ToString());

        var addresses = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>()
            .GetAll(tracking: false)
            .Where(ua => ua.UserId == currentUserId)
            .Select(ua => new UserAddressListDto(
                ua.Id,
                ua.UserId,
                ua.AddressLine
            ))
            .ToListAsync(cancellationToken);

        return Result<GetAllUserAddressQueryResponse>.Success(new GetAllUserAddressQueryResponse(addresses));
    }
}