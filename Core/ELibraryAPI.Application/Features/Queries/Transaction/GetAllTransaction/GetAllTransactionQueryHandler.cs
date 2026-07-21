using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Transaction.GetAllTransaction;

public sealed class GetAllTransactionQueryHandler
    : IRequestHandler<GetAllTransactionQueryRequest, Result<GetAllTransactionQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTransactionQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<GetAllTransactionQueryResponse>> Handle(
        GetAllTransactionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Transaction, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var transactions = await query
            .OrderByDescending(t => t.CreatedDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(t => new TransactionListDto(
                t.Id,
                t.OrderId,
                t.Amount,
                t.Status,
                t.Status == TransactionStatus.Success, 
                t.CreatedDate
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllTransactionQueryResponse>.Success(
            new GetAllTransactionQueryResponse(transactions, totalCount, request.Page, request.Size, totalPages));
    }
}
