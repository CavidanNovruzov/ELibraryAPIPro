using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;

public sealed class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommandRequest, Result<CreatePublisherCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreatePublisherCommandResponse>> Handle(CreatePublisherCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Publisher, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Publisher, Guid>();

        var exists = await readRepo.ExistsAsync(
                    predicate: x => x.Name.ToLower() == request.Name.ToLower().Trim(),
                    tracking: false,
                    ct: ct);

        if (exists)
            return Result<CreatePublisherCommandResponse>.Failure("A publisher with this name already exists.");

        var publisher = _mapper.Map<Domain.Entities.Concrete.Publisher>(request);

        await writeRepo.AddAsync(publisher, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreatePublisherCommandResponse>.Success(
            new CreatePublisherCommandResponse(publisher.Id),
            "Publisher created successfully.");
    }
}