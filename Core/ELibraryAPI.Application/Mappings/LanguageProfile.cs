using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Language.CreateLanguage;
using ELibraryAPI.Application.Features.Commands.Language.UpdateLanguage;
using ELibraryAPI.Application.Features.Queries.Language.GetAllLanguage;
using ELibraryAPI.Application.Features.Queries.Language.GetByIdLanguage;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class LanguageProfile : Profile
{
    public LanguageProfile()
    {
        CreateMap<CreateLanguageCommandRequest, Language>();
        CreateMap<Language, CreateLanguageCommandResponse>();
        CreateMap<UpdateLanguageCommandRequest, Language>();
        CreateMap<Language, UpdateLanguageCommandResponse>();

        //CreateMap<Language, LanguageListDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

        //CreateMap<Language, LanguageDetailDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
    }
}