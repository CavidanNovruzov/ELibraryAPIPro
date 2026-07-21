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

        //CreateMap<Basket, BasketListDto>()
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : ""))
        //    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.BasketItems.Sum(bi => (bi.Product != null ? bi.Product.SalePrice : 0) * bi.Quantity)))
        //    .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.BasketItems.Count));

        //CreateMap<Basket, BasketDetailDto>()
        //    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.BasketItems.Sum(bi => (bi.Product != null ? bi.Product.SalePrice : 0) * bi.Quantity)))
        //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BasketItems));
    }
}