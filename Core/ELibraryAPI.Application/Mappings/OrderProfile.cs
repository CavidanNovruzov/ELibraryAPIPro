using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Order.CreateOrder;
using ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;
using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;
using ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderCommandRequest, Order>();
        CreateMap<Order, CreateOrderCommandResponse>();
        CreateMap<UpdateOrderCommandRequest, Order>()
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore()) 
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); 

        //        CreateMap<Order, OrderListDto>()
        //    .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name))
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
        //    .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.OrderItems.Count));

        //CreateMap<Order, OrderDetailDto>()
        //    .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name))
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
        //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        //CreateMap<OrderItem, OrderItemDetailDto>()
        //    .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
        //    .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));
    }
}