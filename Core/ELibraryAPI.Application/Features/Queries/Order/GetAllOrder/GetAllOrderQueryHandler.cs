using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;

public sealed class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQueryRequest, Result<GetAllOrderQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetAllOrderQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetAllOrderQueryResponse>> Handle(GetAllOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = _currentUserService.IsInRole("Admin");
        var query = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>().GetAll(tracking: false);

        if (isAdmin)
        {
            query = query.IgnoreQueryFilters();
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.CreatedDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(o => new OrderListDto(
                o.Id,
                o.OrderNumber,
                o.CreatedDate,
                o.TotalAmount,
                o.OrderStatus.Name,
                o.User.Email,
                o.OrderItems.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllOrderQueryResponse>.Success(
            new GetAllOrderQueryResponse(orders, totalCount, request.Page, request.Size, totalPages));
    }
}