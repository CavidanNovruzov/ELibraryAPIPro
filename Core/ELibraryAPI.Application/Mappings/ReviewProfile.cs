using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Review.ApproveReview;
using ELibraryAPI.Application.Features.Commands.Review.CreateReview;
using ELibraryAPI.Application.Features.Commands.Review.UpdateReview;
using ELibraryAPI.Application.Features.Queries.Review.GetAllReview;
using ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewCommandRequest, Review>()
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false)) 
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        CreateMap<UpdateReviewCommandRequest, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false));

        CreateMap<Review, ApproveReviewCommandResponse>();

        CreateMap<Review, CreateReviewCommandResponse>();
        CreateMap<Review, UpdateReviewCommandResponse>();

        //CreateMap<Review, ReviewListDto>()
        //    .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : ""))
        //    .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src =>
        //        src.Product != null
        //            ? (src.Product.Images.Where(i => i.IsMain).Select(i => i.ImageUrl).FirstOrDefault() ?? "")
        //            : ""))
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : ""));

        //CreateMap<Review, ReviewDetailDto>()
        //    .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : ""))
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : ""));
    }
}