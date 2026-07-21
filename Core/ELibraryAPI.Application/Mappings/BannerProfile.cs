using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;
using ELibraryAPI.Application.Features.Commands.Banner.UpdateBanner;
using ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class BannerProfile : Profile
{
    public BannerProfile()
    {
        CreateMap<CreateBannerCommandRequest, Banner>();
        CreateMap<Banner, CreateBannerCommandResponse>();

        CreateMap<UpdateBannerCommandRequest, Banner>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<Banner, UpdateBannerCommandResponse>();

        CreateMap<Banner, BannerListDto>();
    }
}