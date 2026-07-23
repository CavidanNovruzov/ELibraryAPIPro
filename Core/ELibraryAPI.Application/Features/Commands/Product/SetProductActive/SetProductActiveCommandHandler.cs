using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.SetProductActive;

public sealed class SetProductActiveCommandHandler : IRequestHandler<SetProductActiveCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly IMediator _mediator;
    public SetProductActiveCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _mediator = mediator;
    }

    public async Task<Result> Handle(SetProductActiveCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Product, Guid>();

        var product = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (product == null)
            return Result.Failure("Məhsul tapılmadı..");

        product.IsActive = request.IsActive;
        writeRepo.Update(product);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("product", request.Id), ct);

        return Result.Success(request.IsActive ? "Məhsul aktivləşdirildi." : "Məhsul deaktiv edildi.");
    }
}

