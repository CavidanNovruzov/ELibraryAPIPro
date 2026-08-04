using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;

public sealed class CreateBranchWorkHoursCommandHandler : IRequestHandler<CreateBranchWorkHoursCommandRequest, Result<CreateBranchWorkHoursCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateBranchWorkHoursCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateBranchWorkHoursCommandResponse>> Handle(CreateBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        if (!_currentUserService.IsAdmin)
            return Result<CreateBranchWorkHoursCommandResponse>.Failure("Filial iş saatlarını dəyişmək üçün inzibatçı hüququnuz olmalıdır.", ErrorType.Forbidden);

        if (request.CloseTime <= request.OpenTime)
            return Result<CreateBranchWorkHoursCommandResponse>.Failure("Bağlanış saatı açılış saatından sonra olmalıdır.", ErrorType.ValidationError);

        var branchReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();
        var workHoursReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();
        var workHoursWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();

        var branchExists = await branchReadRepo.ExistsAsync(x => x.Id == request.BranchId, tracking: false, ct: ct);
        if (!branchExists)
            return Result<CreateBranchWorkHoursCommandResponse>.Failure("Filial tapılmadı.", ErrorType.NotFound);

        var isExist = await workHoursReadRepo
            .GetWhere(x => x.BranchId == request.BranchId && x.Day == request.Day, tracking: false)
            .AnyAsync(ct);

        if (isExist)
            return Result<CreateBranchWorkHoursCommandResponse>.Failure("Bu filial üçün həmin gün üzrə iş saatları artıq mövcuddur.", ErrorType.Conflict);

        var workHours = _mapper.Map<Domain.Entities.Concrete.BranchWorkHours>(request);

        await workHoursWriteRepo.AddAsync(workHours, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateBranchWorkHoursCommandResponse>.Success(
            new CreateBranchWorkHoursCommandResponse(workHours.Id),
            "Filial iş saatları uğurla əlavə edildi.");
    }
}