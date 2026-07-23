using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.DeleteProduct;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly IMediator _mediator;


    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteProductCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Product, Guid>();

        var product = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (product == null)
        {
            return Result.Failure("Məhsul tapılmadı..");
        }

        writeRepository.Remove(product);

        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("product", request.Id), ct);

        return Result.Success("Məhsul uğurla silindi.");
    }
}