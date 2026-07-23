using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.UpdateAuthor;

public sealed class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommandRequest, Result<UpdateAuthorCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateAuthorCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<UpdateAuthorCommandResponse>> Handle(UpdateAuthorCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>();

        var author = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (author == null)
            return Result<UpdateAuthorCommandResponse>.Failure("Müəllif tapılmadı.");

        if (!author.FullName.Equals(request.FullName, StringComparison.OrdinalIgnoreCase))
        {
            var isNameUsed = await readRepo.ExistsAsync(
                x => x.FullName.ToLower() == request.FullName.ToLower(),
                tracking: false,
                ct: ct);

            if (isNameUsed)
                return Result<UpdateAuthorCommandResponse>.Conflict("Bu adda müəllif artıq mövcuddur.");
        }

        _mapper.Map(request, author);

        var saveResult = await _unitOfWork.SaveAsync(ct);

        if (saveResult > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("author", author.Id), ct);

            return Result<UpdateAuthorCommandResponse>.Success(
                new UpdateAuthorCommandResponse(author.Id),
                "Müəllif məlumatları uğurla yeniləndi.");
        }

        return Result<UpdateAuthorCommandResponse>.Failure("Yeniləmə zamanı xəta baş verdi və ya heç bir dəyişiklik tətbiq edilmədi.");
    }
}