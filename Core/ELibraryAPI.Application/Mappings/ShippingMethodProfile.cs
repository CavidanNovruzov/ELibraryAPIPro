using AutoMapper;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;
using ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class ShippingMethodProfile : Profile
{
    public ShippingMethodProfile()
    {
        CreateMap<CreateShippingMethodCommandRequest, ShippingMethod>();
        CreateMap<ShippingMethod, CreateShippingMethodCommandResponse>();
        CreateMap<UpdateShippingMethodCommandRequest, ShippingMethod>();
        CreateMap<ShippingMethod, UpdateShippingMethodCommandResponse>();

        CreateMap<ShippingMethod, ShippingMethodListDto>();

    }
}