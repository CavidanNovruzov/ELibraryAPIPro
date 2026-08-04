using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Basket.CreateBasket;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CreateBasketCommandRequest, Basket>();
        CreateMap<Basket, CreateBasketCommandResponse>();
    }
}