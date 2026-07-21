using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Product.CreateProduct;
using ELibraryAPI.Application.Features.Commands.Product.UpdateProduct;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductCommandRequest, Product>()
            .ForMember(dest => dest.ProductAuthors, opt => opt.Ignore())
            .ForMember(dest => dest.ProductGenres,  opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags,    opt => opt.Ignore())
            .ForMember(dest => dest.Images,         opt => opt.Ignore())
            .AfterMap((_, dest) =>
            {
                if (dest.Id == Guid.Empty)
                    dest.Id = Guid.NewGuid();
            });

        CreateMap<Product, CreateProductCommandResponse>();

        CreateMap<UpdateProductCommandRequest, Product>()
            .ForMember(dest => dest.ProductAuthors, opt => opt.Ignore())
            .ForMember(dest => dest.ProductGenres,  opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags,    opt => opt.Ignore())
            .ForMember(dest => dest.Images,         opt => opt.Ignore());

        CreateMap<Product, UpdateProductCommandResponse>();
    }
}
