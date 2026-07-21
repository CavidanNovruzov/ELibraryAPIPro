using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Stock.CreateStock;
using ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public sealed class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<CreateStockCommandRequest, Stock>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateStockCommandRequest, Stock>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Stock, CreateStockCommandResponse>();
        CreateMap<Stock, UpdateStockCommandResponse>();
    }
}