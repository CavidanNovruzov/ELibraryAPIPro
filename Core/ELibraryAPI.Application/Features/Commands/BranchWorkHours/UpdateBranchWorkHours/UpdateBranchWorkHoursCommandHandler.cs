using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.UpdateBranchWorkHours;

public sealed class UpdateBranchWorkHoursCommandHandler : IRequestHandler<UpdateBranchWorkHoursCommandRequest, Result<UpdateBranchWorkHoursCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBranchWorkHoursCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateBranchWorkHoursCommandResponse>> Handle(UpdateBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();

        var workHours = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (workHours == null)
        {
            return Result<UpdateBranchWorkHoursCommandResponse>.Failure("Work hours not found.");
        }

        if (request.OpenTime >= request.CloseTime)
        {
            return Result<UpdateBranchWorkHoursCommandResponse>.Failure("Opening time cannot be later than or equal to closing time.");
        }

        if (workHours.BranchId != request.BranchId || workHours.Day != request.Day)
        {
            var isDuplicate = await readRepo.ExistsAsync(
                x => x.BranchId == request.BranchId && x.Day == request.Day && x.Id != request.Id,
                tracking: false,
                ct: ct);

            if (isDuplicate)
            {
                return Result<UpdateBranchWorkHoursCommandResponse>.Failure("Work hours for this day already exist for the selected branch.");
            }
        }

        _mapper.Map(request, workHours);

        writeRepo.Update(workHours);
        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateBranchWorkHoursCommandResponse>.Success(
            new UpdateBranchWorkHoursCommandResponse(workHours.Id),
            "Branch work hours updated successfully.");
    }
}