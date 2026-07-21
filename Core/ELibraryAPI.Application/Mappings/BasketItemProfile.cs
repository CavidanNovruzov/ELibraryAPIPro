using AutoMapper;
using ELibraryAPI.Application.Features.Commands.BasketItem.CreateBasketItem;
using ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItem;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class BasketItemProfile : Profile
{
    public BasketItemProfile()
    {
        CreateMap<CreateBasketItemCommandRequest, BasketItem>();
        CreateMap<BasketItem, CreateBasketItemCommandResponse>();
        CreateMap<UpdateBasketItemQuantityRequest, BasketItem>();
        CreateMap<BasketItem, UpdateBasketItemQuantityResponse>();
    }
}
