using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.CoverType.CreateCoverType;

public sealed class CreateCoverTypeCommandHandler : IRequestHandler<CreateCoverTypeCommandRequest, Result<CreateCoverTypeCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCoverTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateCoverTypeCommandResponse>> Handle(CreateCoverTypeCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.CoverType, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.CoverType, Guid>();

        var isNameExists = await readRepo.ExistsAsync(
            x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isNameExists)
        {
            return Result<CreateCoverTypeCommandResponse>.Failure("Bu üz qabığı növü artıq mövcuddur.");
        }

        var coverType = _mapper.Map<Domain.Entities.Concrete.CoverType>(request);

        await writeRepo.AddAsync(coverType, ct);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<CreateCoverTypeCommandResponse>.Success(
                new CreateCoverTypeCommandResponse(coverType.Id),
                "Əməliyyat uğurla tamamlandı.");
        }

        return Result<CreateCoverTypeCommandResponse>.Failure("Üz qabığı növü yaradılarkən xəta baş verdi.");
    }
}