using AutoMapper;
using ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;
using ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;
using ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class PromoCodeProfile : Profile
{
    public PromoCodeProfile()
    {
        CreateMap<CreatePromoCodeCommandRequest, PromoCode>();
        CreateMap<PromoCode, CreatePromoCodeCommandResponse>();
        CreateMap<UpdatePromoCodeCommandRequest, PromoCode>();
        CreateMap<PromoCode, UpdatePromoCodeCommandResponse>();

        CreateMap<PromoCode, PromoCodeListDto>();
    }
}