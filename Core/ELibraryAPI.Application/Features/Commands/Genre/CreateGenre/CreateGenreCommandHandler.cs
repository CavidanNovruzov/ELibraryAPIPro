using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Genre.CreateGenre;

public sealed class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommandRequest, Result<CreateGenreCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGenreCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateGenreCommandResponse>> Handle(CreateGenreCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Genre, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Genre, Guid>();

        var exists = await readRepo.ExistsAsync(
            predicate: x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (exists)
        {
            return Result<CreateGenreCommandResponse>.Failure("A genre with this name already exists.");
        }

        var genre = _mapper.Map<Domain.Entities.Concrete.Genre>(request);

        await writeRepo.AddAsync(genre, ct);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<CreateGenreCommandResponse>.Success(
                new CreateGenreCommandResponse(genre.Id),
                "Genre created successfully.");
        }

        return Result<CreateGenreCommandResponse>.Failure("An error occurred while creating the genre.");
    }
}