using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;
using ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;
using ELibraryAPI.Application.Features.Queries.Publisher.GetAllPublisher;
using ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<CreatePublisherCommandRequest, Publisher>();
        CreateMap<Publisher, CreatePublisherCommandResponse>();
        CreateMap<UpdatePublisherCommandRequest, Publisher>();
        CreateMap<Publisher, UpdatePublisherCommandResponse>();

        //CreateMap<Publisher, PublisherListDto>()
        //    .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Products.Count));

        //CreateMap<Publisher, PublisherDetailDto>()
        //    .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Products.Count));
    }
}