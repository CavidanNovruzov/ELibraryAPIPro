using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;

public sealed class GetAllPaymentMethodQueryHandler : IRequestHandler<GetAllPaymentMethodQueryRequest, Result<GetAllPaymentMethodQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPaymentMethodQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllPaymentMethodQueryResponse>> Handle(GetAllPaymentMethodQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.PaymentMethod, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var paymentMethods = await query
            .OrderBy(pm => pm.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(pm => new PaymentMethodListDto(
                pm.Id,
                pm.Name
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllPaymentMethodQueryResponse>.Success(
            new GetAllPaymentMethodQueryResponse(paymentMethods, totalCount, request.Page, request.Size, totalPages));
    }
}