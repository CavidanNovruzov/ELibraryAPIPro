using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Banner.GetActiveBanners;

public sealed class GetActiveBannersQueryHandler : IRequestHandler<GetActiveBannersQueryRequest, Result<List<GetActiveBannersQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActiveBannersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetActiveBannersQueryResponse>>> Handle(GetActiveBannersQueryRequest request, CancellationToken ct)
    {
        var bannerReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Banner, Guid>();

        var banners = await bannerReadRepo.GetAll(tracking: false)
            .Where(b => b.IsActive)
            .OrderBy(b => b.Order)
            .Select(b => new GetActiveBannersQueryResponse(
                b.Id,
                b.Title,
                b.ImageUrl,
                b.Order
            ))
            .ToListAsync(ct);

        return Result<List<GetActiveBannersQueryResponse>>.Success(banners);
    }
}