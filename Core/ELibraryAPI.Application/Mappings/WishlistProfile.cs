using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;
using ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.MoveToBasket;
using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;


namespace ELibraryAPI.Application.Mappings;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<CreateWishlistCommandRequest, Domain.Entities.Concrete.Wishlist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CreateWishlistItemCommandRequest, Domain.Entities.Concrete.WishlistItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateWishlistItemCommandRequest, Domain.Entities.Concrete.WishlistItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<MoveToBasketCommandRequest, Domain.Entities.Concrete.BasketItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

    }
}