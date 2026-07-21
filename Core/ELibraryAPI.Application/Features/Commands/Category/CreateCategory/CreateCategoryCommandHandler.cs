using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, Result<CreateCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
            return Result<CreateCategoryCommandResponse>.Failure("A category with this name already exists.");
        }

        var category = _mapper.Map<Domain.Entities.Concrete.Category>(request);

        await writeRepo.AddAsync(category, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateCategoryCommandResponse>.Success(
            new CreateCategoryCommandResponse(category.Id),
            "Category created successfully.");
    }
}
