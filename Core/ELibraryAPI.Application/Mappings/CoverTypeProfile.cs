using AutoMapper;
using ELibraryAPI.Application.Features.Commands.CoverType.CreateCoverType;
using ELibraryAPI.Application.Features.Commands.CoverType.UpdateCoverType;
using ELibraryAPI.Application.Features.Queries.CoverType.GetAllCoverType;
using ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class CoverTypeProfile : Profile
{
    public CoverTypeProfile()
    {
        CreateMap<CreateCoverTypeCommandRequest, CoverType>();
        CreateMap<CoverType, CreateCoverTypeCommandResponse>();
        CreateMap<UpdateCoverTypeCommandRequest, CoverType>();
        CreateMap<CoverType, UpdateCoverTypeCommandResponse>();

        //CreateMap<CoverType, CoverTypeListDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

        //CreateMap<CoverType, CoverTypeDetailDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
    }
}