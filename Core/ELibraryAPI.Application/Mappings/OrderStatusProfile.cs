using AutoMapper;
using ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;
using ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;
using ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class OrderStatusProfile : Profile
{
    public OrderStatusProfile()
    {
        CreateMap<CreateOrderStatusCommandRequest, OrderStatus>();
        CreateMap<OrderStatus, CreateOrderStatusCommandResponse>();
        CreateMap<UpdateOrderStatusCommandRequest, OrderStatus>();
        CreateMap<OrderStatus, UpdateOrderStatusCommandResponse>();

        CreateMap<OrderStatus, OrderStatusListDto>();

    }
}