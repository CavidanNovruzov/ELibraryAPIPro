using AutoMapper;
using ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public sealed class UserSearchHistoryProfile : Profile
{
    public UserSearchHistoryProfile()
    {
        CreateMap<CreateUserSearchHistoryCommandRequest, UserSearchHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UserSearchHistory, CreateUserSearchHistoryCommandResponse>();

    }
}