using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;

public sealed class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommandRequest, Result<CreateAuthorCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateAuthorCommandResponse>> Handle(CreateAuthorCommandRequest request, CancellationToken ct)
    {
        var exists = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .ExistsAsync(a => a.FullName == request.FullName, tracking: false, ct: ct);

        if (exists)
            return Result<CreateAuthorCommandResponse>.Failure("An author with the same name already exists.");

        var author = _mapper.Map<Domain.Entities.Concrete.Author>(request);

        await _unitOfWork.WriteRepository<Domain.Entities.Concrete.Author, Guid>().AddAsync(author, ct);

 
        var saveResult = await _unitOfWork.SaveAsync(ct);

        if (saveResult > 0)
        {
            return Result<CreateAuthorCommandResponse>.Success(new CreateAuthorCommandResponse(author.Id));
        }

        return Result<CreateAuthorCommandResponse>.Failure("An error occurred while creating the author.");
    }

}
