using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.UpdateLanguage;

public sealed class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommandRequest, Result<UpdateLanguageCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLanguageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateLanguageCommandResponse>> Handle(UpdateLanguageCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Language, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Language, Guid>();

        var language = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (language == null)
            return Result<UpdateLanguageCommandResponse>.Failure("Language not found.");

        if (language.Code.ToLower() != request.Code.Trim().ToLower() || language.Name.ToLower() != request.Name.Trim().ToLower())
        {
            var isAlreadyTaken = await readRepo.ExistsAsync(
                x => (x.Code.ToLower() == request.Code.Trim().ToLower() || x.Name.ToLower() == request.Name.Trim().ToLower())
                     && x.Id != request.Id,
                tracking: false, ct: ct);

            if (isAlreadyTaken)
                return Result<UpdateLanguageCommandResponse>.Failure("Another language with the same name or code already exists.");
        }

        _mapper.Map(request, language);

        writeRepo.Update(language);
        var saveResult = await _unitOfWork.SaveAsync(ct);

        if (saveResult > 0)
        {
            return Result<UpdateLanguageCommandResponse>.Success(
                new UpdateLanguageCommandResponse(language.Id),
                "Language updated successfully.");
        }

        return Result<UpdateLanguageCommandResponse>.Failure("No changes were saved.");
    }
}