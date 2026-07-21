using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;

public sealed class GetAllShippingMethodQueryHandler : IRequestHandler<GetAllShippingMethodQueryRequest, Result<GetAllShippingMethodQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllShippingMethodQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllShippingMethodQueryResponse>> Handle(GetAllShippingMethodQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.ShippingMethod, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var methods = await query
            .OrderBy(sm => sm.Price)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(sm => new ShippingMethodListDto(
                sm.Id,
                sm.Name,
                sm.Price
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllShippingMethodQueryResponse>.Success(
            new GetAllShippingMethodQueryResponse(methods, totalCount, request.Page, request.Size, totalPages));
    }
}