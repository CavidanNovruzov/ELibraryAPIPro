using AutoMapper;
using ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;
using ELibraryAPI.Application.Features.Commands.BranchWorkHours.UpdateBranchWorkHours;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Mappings;

public class BranchWorkHoursProfile : Profile
{
    public BranchWorkHoursProfile()
    {
        CreateMap<CreateBranchWorkHoursCommandRequest, BranchWorkHours>();
        CreateMap<BranchWorkHours, CreateBranchWorkHoursCommandResponse>();
        CreateMap<UpdateBranchWorkHoursCommandRequest, BranchWorkHours>();
        CreateMap<BranchWorkHours, UpdateBranchWorkHoursCommandResponse>();
    }
}
