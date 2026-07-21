using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.CreateLanguage;

public sealed class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommandRequest, Result<CreateLanguageCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLanguageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateLanguageCommandResponse>> Handle(CreateLanguageCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Language, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Language, Guid>();

        var isExists = await readRepo.ExistsAsync(
            x => x.Code.ToLower() == request.Code.Trim().ToLower() || x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isExists)
        {
            return Result<CreateLanguageCommandResponse>.Failure("Language with this name or code already exists.");
        }

        var language = _mapper.Map<Domain.Entities.Concrete.Language>(request);

        await writeRepo.AddAsync(language, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateLanguageCommandResponse>.Success(
            new CreateLanguageCommandResponse(language.Id),
            "Language created successfully.");
    }
}