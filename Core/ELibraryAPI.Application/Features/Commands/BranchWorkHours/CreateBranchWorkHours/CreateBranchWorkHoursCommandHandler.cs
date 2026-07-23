using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;

public sealed class CreateBranchWorkHoursCommandHandler : IRequestHandler<CreateBranchWorkHoursCommandRequest, Result<CreateBranchWorkHoursCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBranchWorkHoursCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateBranchWorkHoursCommandResponse>> Handle(CreateBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();

        var isExist = await readRepo.GetWhere(x => x.BranchId == request.BranchId && x.Day == request.Day).AnyAsync(ct);

        if (isExist)
            return Result<CreateBranchWorkHoursCommandResponse>.Conflict("Bu filial üçün həmin gün üzrə iş saatları artıq mövcuddur.");

        if (isExist)
        {
            return Result<CreateBranchWorkHoursCommandResponse>.Conflict("Bu filial üçün həmin gün üzrə iş saatları artıq mövcuddur.");
        }

        var workHours = _mapper.Map<Domain.Entities.Concrete.BranchWorkHours>(request);

        await writeRepo.AddAsync(workHours, ct);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<CreateBranchWorkHoursCommandResponse>.Success(new CreateBranchWorkHoursCommandResponse(workHours.Id), "Əməliyyat uğurla tamamlandı.");

        return Result<CreateBranchWorkHoursCommandResponse>.Failure("Filial iş saatları yadda saxlanılarkən xəta baş verdi.");
    }
}