using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;


public sealed class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommandRequest, Result<UpdateTagCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTagCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateTagCommandResponse>> Handle(UpdateTagCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Tag, Guid>();

        var tag = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (tag == null)
            return Result<UpdateTagCommandResponse>.Failure("Etiket tapılmadı.");

        var normalizedName = request.Name.Trim();
        if (tag.Name.ToLower() != normalizedName.ToLower())
        {
            var exists = await readRepo.ExistsAsync(x => x.Name.ToLower() == normalizedName.ToLower(), false, ct);
            if (exists)
                return Result<UpdateTagCommandResponse>.Failure("Bu adda teq artıq mövcuddur.");
        }

        _mapper.Map(request, tag);
        tag.Name = normalizedName;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateTagCommandResponse>.Success(new(tag.Id), "Əməliyyat uğurla tamamlandı.");
    }
}