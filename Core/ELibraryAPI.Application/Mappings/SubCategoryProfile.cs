using AutoMapper;
using ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;
using ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;
using ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetAllSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class SubCategoryProfile : Profile
{
    public SubCategoryProfile()
    {
        CreateMap<CreateSubCategoryCommandRequest, SubCategory>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateSubCategoryCommandRequest, SubCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<SubCategory, CreateSubCategoryCommandResponse>();
        CreateMap<SubCategory, UpdateSubCategoryCommandResponse>();

        CreateMap<SubCategory, MergeSubCategoriesCommandResponse>();

        CreateMap<SubCategory, SubCategoryListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

        CreateMap<SubCategory, SubCategoryDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
    }
}