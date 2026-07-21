using AutoMapper;
using ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;
using ELibraryAPI.Application.Features.Commands.UserAddress.UpdateUserAddress;
using ELibraryAPI.Application.Features.Queries.UserAddress.GetAllUserAddress;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class UserAddressProfile : Profile
{
    public UserAddressProfile()
    {

        CreateMap<CreateUserAddressCommandRequest, UserAddress>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.City, opt => opt.Ignore());

        CreateMap<UpdateUserAddressCommandRequest, UserAddress>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<UserAddress, CreateUserAddressCommandResponse>();
        CreateMap<UserAddress, UpdateUserAddressCommandResponse>();

        CreateMap<UserAddress, UserAddressListDto>();

    }
}