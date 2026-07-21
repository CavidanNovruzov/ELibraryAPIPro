using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetByIdCampaign;

public sealed class GetByIdCampaignQueryHandler : IRequestHandler<GetByIdCampaignQueryRequest, Result<GetByIdCampaignQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdCampaignQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdCampaignQueryResponse>> Handle(GetByIdCampaignQueryRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var campaign = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Campaign, Guid>()
            .GetAll(tracking: false)
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .Select(c => new CampaignDetailDto(
                c.Id,
                c.Title,
                c.Description,
                c.DiscountPercent,
                c.StartDate,
                c.EndDate,
                c.IsActive,
                now > c.EndDate 
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (campaign == null)
            return Result<GetByIdCampaignQueryResponse>.Failure("Kampaniya tapılmadı.");

        return Result<GetByIdCampaignQueryResponse>.Success(new GetByIdCampaignQueryResponse(campaign));
    }
}