using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;
using ELibraryAPI.Application.Features.Commands.Author.UpdateAuthor;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<CreateAuthorCommandRequest, Author>();
        CreateMap<Author, CreateAuthorCommandResponse>();
        CreateMap<UpdateAuthorCommandRequest, Author>();
        CreateMap<Author, UpdateAuthorCommandResponse>();
    }
}
