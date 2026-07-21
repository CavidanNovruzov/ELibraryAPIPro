using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetAllCampaign;

public sealed class GetAllCampaignQueryHandler : IRequestHandler<GetAllCampaignQueryRequest, Result<GetAllCampaignQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService; 

    public GetAllCampaignQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetAllCampaignQueryResponse>> Handle(GetAllCampaignQueryRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // 1. İlkin sorğunu (Dataset) götürürük
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Campaign, Guid>()
            .GetAll(tracking: false);

        bool isAdmin = _currentUserService.IsAdmin;

        if (!isAdmin)
        {
            query = query.Where(c => c.IsActive && c.StartDate <= now && c.EndDate >= now);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var campaigns = await query
            .OrderByDescending(c => c.StartDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(c => new CampaignListDto(
                c.Id,
                c.Title,
                c.Description,
                c.DiscountPercent,
                c.StartDate,
                c.EndDate,
                c.IsActive && c.StartDate <= now && c.EndDate >= now
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllCampaignQueryResponse>.Success(
            new GetAllCampaignQueryResponse(campaigns, totalCount, request.Page, request.Size, totalPages));
    }
}