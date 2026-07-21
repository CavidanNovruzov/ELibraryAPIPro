using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Tag.CreateTag;
using ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;
using ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;

using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<CreateTagCommandRequest, Tag>()
               .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateTagCommandRequest, Tag>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Tag, CreateTagCommandResponse>();
        CreateMap<Tag, UpdateTagCommandResponse>();

    }
}