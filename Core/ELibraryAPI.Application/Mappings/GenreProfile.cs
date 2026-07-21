using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Genre.CreateGenre;
using ELibraryAPI.Application.Features.Commands.Genre.UpdateGenre;
using ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;
using ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<CreateGenreCommandRequest, Genre>();
        CreateMap<Genre, CreateGenreCommandResponse>();
        CreateMap<UpdateGenreCommandRequest, Genre>();
        CreateMap<Genre, UpdateGenreCommandResponse>();

        //CreateMap<Genre, GenreListDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ProductGenres.Count));

        //CreateMap<Genre, GenreDetailDto>()
        //    .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ProductGenres.Count));
    }
}