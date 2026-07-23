using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;

public sealed class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommandRequest, Result<CreateAuthorCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAuthorCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<CreateAuthorCommandResponse>> Handle(CreateAuthorCommandRequest request, CancellationToken ct)
    {
        var exists = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .ExistsAsync(a => a.FullName == request.FullName, tracking: false, ct: ct);

        if (exists)
            return Result<CreateAuthorCommandResponse>.Conflict("Eyni adlı müəllif artıq mövcuddur.");

        var author = _mapper.Map<Domain.Entities.Concrete.Author>(request);

        await _unitOfWork.WriteRepository<Domain.Entities.Concrete.Author, Guid>().AddAsync(author, ct);

        var saveResult = await _unitOfWork.SaveAsync(ct);

        if (saveResult > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("author", author.Id), ct);

            return Result<CreateAuthorCommandResponse>.Success(
                new CreateAuthorCommandResponse(author.Id),
                "Müəllif uğurla yaradıldı.");
        }

        return Result<CreateAuthorCommandResponse>.Failure("Müəllif yaradılarkən xəta baş verdi.");
    }
}