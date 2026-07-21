using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(DeleteCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Category, Guid>();

        var category = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (category == null)
        {
            return Result.Failure("Category not found or already deleted.");
        }

        category.IsDeleted = true;
        writeRepo.Update(category);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Category moved to archive.");
    }
}
