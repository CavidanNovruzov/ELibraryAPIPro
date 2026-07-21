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
            return Result<UpdateTagCommandResponse>.Failure("Tag not found.");

        var normalizedName = request.Name.Trim();
        if (tag.Name.ToLower() != normalizedName.ToLower())
        {
            var exists = await readRepo.ExistsAsync(x => x.Name.ToLower() == normalizedName.ToLower(), false, ct);
            if (exists)
                return Result<UpdateTagCommandResponse>.Failure("A tag with this name already exists.");
        }

        _mapper.Map(request, tag);
        tag.Name = normalizedName;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateTagCommandResponse>.Success(new(tag.Id), "Tag updated successfully.");
    }
}