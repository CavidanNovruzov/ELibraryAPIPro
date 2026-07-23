using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.UpdateBranch;

public sealed class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommandRequest, Result<UpdateBranchCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateBranchCommandResponse>> Handle(UpdateBranchCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Branch, Guid>();

        var branch = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (branch == null)
            return Result<UpdateBranchCommandResponse>.Failure("Filial tapılmadı.");

        _mapper.Map(request, branch);

        writeRepo.Update(branch);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<UpdateBranchCommandResponse>.Success(new UpdateBranchCommandResponse(branch.Id));

        return Result<UpdateBranchCommandResponse>.Failure("Filialda heç bir dəyişiklik edilmədi.");
    }
}