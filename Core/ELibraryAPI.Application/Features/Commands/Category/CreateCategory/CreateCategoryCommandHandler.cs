using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, Result<CreateCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<CreateCategoryCommandResponse>> Handle(CreateCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Category, Guid>();

        var isNameUsed = await readRepo.ExistsAsync(
            x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isNameUsed)
        {
            return Result<CreateCategoryCommandResponse>.Failure("Bu adda kateqoriya artıq mövcuddur.");
        }

        var category = _mapper.Map<Domain.Entities.Concrete.Category>(request);

        await writeRepo.AddAsync(category, ct);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("category", category.Id), ct);

        return Result<CreateCategoryCommandResponse>.Success(
            new CreateCategoryCommandResponse(category.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}
