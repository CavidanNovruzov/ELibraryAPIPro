using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;

public sealed class GetAllBannerQueryHandler : IRequestHandler<GetAllBannerQueryRequest, Result<GetAllBannerQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBannerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllBannerQueryResponse>> Handle(GetAllBannerQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Banner, Guid>()
            .GetAll(tracking: false)
            .Where(b => b.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);

        var banners = await query
            .OrderBy(b => b.Order)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(b => new BannerListDto(
                b.Id,
                b.ImageUrl,
                b.RedirectUrl,
                b.Title
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllBannerQueryResponse>.Success(
            new GetAllBannerQueryResponse(banners, totalCount, request.Page, request.Size, totalPages));
    }
}