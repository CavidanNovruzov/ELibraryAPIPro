using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.CreateTag;

public sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommandRequest, Result<CreateTagCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateTagCommandResponse>> Handle(CreateTagCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Tag, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Tag, Guid>();

        var normalizedName = request.Name.Trim();

        var isNameExists = await readRepository.ExistsAsync(
            x => x.Name.ToLower() == normalizedName.ToLower(),
            tracking: false,
            ct: ct);

        if (isNameExists)
        {
            return Result<CreateTagCommandResponse>.Failure("A tag with this name already exists.");
        }

        var tag = _mapper.Map<Domain.Entities.Concrete.Tag>(request);
        tag.Name = normalizedName;

        await writeRepository.AddAsync(tag, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateTagCommandResponse>.Success(
            new CreateTagCommandResponse(tag.Id),
            "Tag created successfully.");
    }
}