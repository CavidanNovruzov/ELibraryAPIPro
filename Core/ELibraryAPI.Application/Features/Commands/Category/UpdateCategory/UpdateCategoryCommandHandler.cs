using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, Result<UpdateCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<UpdateCategoryCommandResponse>> Handle(UpdateCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Category, Guid>();

        var category = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (category == null)
            return Result<UpdateCategoryCommandResponse>.Failure("Kateqoriya tapılmadı.");

        if (category.Name.ToLower() != request.Name.Trim().ToLower())
        {
            var isNameExists = await readRepo.ExistsAsync(
                x => x.Name.ToLower() == request.Name.Trim().ToLower() && x.Id != request.Id && !x.IsDeleted,
                tracking: false,
                ct: ct);

            if (isNameExists)
                return Result<UpdateCategoryCommandResponse>.Failure("Bu adda başqa kateqoriya artıq mövcuddur.");
        }

        _mapper.Map(request, category);

        writeRepo.Update(category);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("category", category.Id), ct);

            return Result<UpdateCategoryCommandResponse>.Success(
                new UpdateCategoryCommandResponse(category.Id),
                "Əməliyyat uğurla tamamlandı.");
        }

        return Result<UpdateCategoryCommandResponse>.Failure("Heç bir dəyişiklik tətbiq edilmədi.");
    }
}