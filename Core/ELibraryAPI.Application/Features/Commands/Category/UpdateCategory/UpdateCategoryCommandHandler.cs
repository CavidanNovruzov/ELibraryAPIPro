using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, Result<UpdateCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateCategoryCommandResponse>> Handle(UpdateCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Category, Guid>();

        var category = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (category == null)
            return Result<UpdateCategoryCommandResponse>.Failure("Category not found.");

        if (category.Name.ToLower() != request.Name.Trim().ToLower())
        {
            var isNameExists = await readRepo.ExistsAsync(
                x => x.Name.ToLower() == request.Name.Trim().ToLower() && x.Id != request.Id && !x.IsDeleted,
                tracking: false,
                ct: ct);

            if (isNameExists)
                return Result<UpdateCategoryCommandResponse>.Failure("Another category with this name already exists.");
        }

        _mapper.Map(request, category);

        writeRepo.Update(category);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<UpdateCategoryCommandResponse>.Success(
                new UpdateCategoryCommandResponse(category.Id),
                "Category updated successfully.");
        }

        return Result<UpdateCategoryCommandResponse>.Failure("No changes were applied.");
    }
}