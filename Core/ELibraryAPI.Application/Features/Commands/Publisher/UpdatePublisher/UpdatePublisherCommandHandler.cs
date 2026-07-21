using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;

public sealed class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommandRequest, Result<UpdatePublisherCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdatePublisherCommandResponse>> Handle(UpdatePublisherCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Publisher, Guid>();

        var publisher = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);


        if (publisher == null)
        {
            return Result<UpdatePublisherCommandResponse>.Failure("Publisher not found.");
        }

        var normalizedName = request.Name.Trim().ToLower();
        if (publisher.Name.ToLower() != normalizedName)
        {
            var isNameUsed = await readRepo.ExistsAsync(
                x => x.Name.ToLower() == normalizedName,
                tracking: false,
                ct: ct);

            if (isNameUsed)
                return Result<UpdatePublisherCommandResponse>.Failure("A publisher with this name already exists.");
        }

        _mapper.Map(request, publisher);

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdatePublisherCommandResponse>.Success(
            new UpdatePublisherCommandResponse(publisher.Id),
            "Publisher updated successfully.");
    }
}