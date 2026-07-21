using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.CreateBranch;

public sealed class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommandRequest, Result<CreateBranchCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateBranchCommandResponse>> Handle(CreateBranchCommandRequest request, CancellationToken ct)
    {
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Branch, Guid>();

        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();
        bool isExist = await readRepo.ExistsAsync(x => x.Name.ToLower() == request.Name.ToLower(), ct: ct);

        if (isExist)
            return Result<CreateBranchCommandResponse>.Failure("A branch with this name already exists.");

        var branch = _mapper.Map<ELibraryAPI.Domain.Entities.Concrete.Branch>(request);

        await writeRepo.AddAsync(branch, ct);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<CreateBranchCommandResponse>.Success(new CreateBranchCommandResponse(branch.Id));

        return Result<CreateBranchCommandResponse>.Failure("An error occurred while creating the branch.");
    }
}