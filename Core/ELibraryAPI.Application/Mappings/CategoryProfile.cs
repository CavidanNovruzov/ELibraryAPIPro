using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Category.CreateCategory;
using ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;
using ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;
using ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryCommandRequest, Category>();
        CreateMap<Category, CreateCategoryCommandResponse>();
        CreateMap<UpdateCategoryCommandRequest, Category>();
        CreateMap<Category, UpdateCategoryCommandResponse>();

        //CreateMap<Category, CategoryListDto>()
        //    .ForMember(dest => dest.SubCategoryCount, opt => opt.MapFrom(src => src.SubCategories.Count))
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.SubCategories.Sum(sc => sc.Products.Count)));

        //CreateMap<Category, CategoryDetailDto>()
        //    .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));
    }
}