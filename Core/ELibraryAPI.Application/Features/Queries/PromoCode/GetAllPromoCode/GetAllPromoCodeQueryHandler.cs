using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;

public sealed class GetAllPromoCodeQueryHandler : IRequestHandler<GetAllPromoCodeQueryRequest, Result<GetAllPromoCodeQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPromoCodeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllPromoCodeQueryResponse>> Handle(GetAllPromoCodeQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var promoCodes = await query
            .OrderByDescending(pc => pc.StartDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(pc => new PromoCodeListDto(
                pc.Id,
                pc.Code,
                pc.DiscountPercent,
                pc.StartDate,
                pc.EndDate,
                pc.UsageLimit
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllPromoCodeQueryResponse>.Success(
            new GetAllPromoCodeQueryResponse(promoCodes, totalCount, request.Page, request.Size, totalPages));
    }
}