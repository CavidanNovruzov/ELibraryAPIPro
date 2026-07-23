using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Product.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, Result<UpdateProductCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<UpdateProductCommandResponse>> Handle(UpdateProductCommandRequest request, CancellationToken ct)
    {
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var product = await productReadRepo.GetAll(tracking: true) 
            .Include(p => p.ProductAuthors)
            .Include(p => p.ProductGenres)
            .Include(p => p.ProductTags)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product == null)
            return Result<UpdateProductCommandResponse>.Failure("Məhsul tapılmadı..");

        if (product.ISBN != request.ISBN.Trim())
        {
            var isIsbnExists = await productReadRepo.ExistsAsync(
                x => x.ISBN == request.ISBN.Trim() && x.Id != request.Id, false, ct);

            if (isIsbnExists)
                return Result<UpdateProductCommandResponse>.Failure("Bu ISBN nömrəli məhsul artıq mövcuddur.");
        }

        if (product.SalePrice != request.SalePrice)
        {
            var priceHistory = new PriceHistory
            {
                ProductId = product.Id,
                OldPrice = product.SalePrice,
                NewPrice = request.SalePrice,
                ChangeReason = "Product update via Admin Panel"
            };
            await _unitOfWork.WriteRepository<PriceHistory, Guid>().AddAsync(priceHistory, ct);
        }

        product.ProductAuthors.Clear();
        product.ProductGenres.Clear();
        product.ProductTags.Clear();
        product.Images.Clear();


        request.AuthorIds?.ForEach(aId => product.ProductAuthors.Add(new ProductAuthor { AuthorId = aId }));
        request.GenreIds?.ForEach(gId => product.ProductGenres.Add(new ProductGenre { GenreId = gId }));
        request.TagIds?.ForEach(tId => product.ProductTags.Add(new ProductTag { TagId = tId }));
        request.Images?.ForEach(img => product.Images.Add(new ProductImage { ImageUrl = img.ImageUrl, IsMain = img.IsMain }));

 
        _mapper.Map(request, product);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("product", product.Id), ct);

        return Result<UpdateProductCommandResponse>.Success(
            new UpdateProductCommandResponse(product.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}