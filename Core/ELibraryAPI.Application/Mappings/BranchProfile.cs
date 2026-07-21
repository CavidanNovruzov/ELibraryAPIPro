using AutoMapper;
using ELibraryAPI.Application.Features.Commands.Branch.CreateBranch;
using ELibraryAPI.Application.Features.Commands.Branch.UpdateBranch;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<CreateBranchCommandRequest, Branch>();
        CreateMap<Branch, CreateBranchCommandResponse>();
        CreateMap<UpdateBranchCommandRequest, Branch>();
        CreateMap<Branch, UpdateBranchCommandResponse>();
    }
}
