using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Campaign.CreateCampaign;
using ELibraryAPI.Application.Features.Commands.Campaign.UpdateCampaign;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class CampaignProfile : Profile
{
    public CampaignProfile()
    {
        CreateMap<CreateCampaignCommandRequest, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCampaigns, opt => opt.Ignore());

        CreateMap<UpdateCampaignCommandRequest, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCampaigns, opt => opt.Ignore());

        CreateMap<Campaign, CreateCampaignCommandResponse>();
        CreateMap<Campaign, UpdateCampaignCommandResponse>();
    }
}
