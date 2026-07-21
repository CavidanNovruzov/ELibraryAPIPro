using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore; 


namespace ELibraryAPI.Application.Features.Commands.Product.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, Result<CreateProductCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request, CancellationToken ct)
    {
        var productReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var productWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Product, Guid>();

        var isIsbnExists = await productReadRepository.ExistsAsync(
            x => x.ISBN == request.ISBN.Trim(),
            tracking: false,
            ct: ct);

        if (isIsbnExists)
        {
            return Result<CreateProductCommandResponse>.Failure("A product with this ISBN already exists.");
        }

        var product = _mapper.Map<Domain.Entities.Concrete.Product>(request);

        var authorIds = request.AuthorIds?.Distinct().ToList() ?? new List<Guid>();
        if (authorIds.Any())
        {
            var authorRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>();

 
            var existingAuthorsCount = await authorRepo
                .GetAll(tracking: false)
                .Where(a => authorIds.Contains(a.Id))
                .CountAsync(ct);

            if (existingAuthorsCount != authorIds.Count)
            {
                return Result<CreateProductCommandResponse>.Failure(
                    "One or more provided Author IDs were not found.",
                    ErrorType.NotFound);
            }

            foreach (var authorId in authorIds)
            {
                product.ProductAuthors.Add(new Domain.Entities.Concrete.ProductAuthor
                {
                    AuthorId = authorId,
                    ProductId = product.Id
                });
            }
        }

        // 3. Janrların (Genres) Əlavə Edilməsi
        if (request.GenreIds != null && request.GenreIds.Any())
        {
            foreach (var genreId in request.GenreIds)
            {
                product.ProductGenres.Add(new Domain.Entities.Concrete.ProductGenre
                {
                    GenreId = genreId,
                    ProductId = product.Id
                });
            }
        }

        // 4. Teqlərin (Tags) Əlavə Edilməsi
        if (request.TagIds != null && request.TagIds.Any())
        {
            foreach (var tagId in request.TagIds)
            {
                product.ProductTags.Add(new Domain.Entities.Concrete.ProductTag
                {
                    TagId = tagId,
                    ProductId = product.Id
                });
            }
        }

        // 5. Şəkillərin (Images) Əlavə Edilməsi
        if (request.Images != null && request.Images.Any())
        {
            foreach (var imageDto in request.Images)
            {
                product.Images.Add(new Domain.Entities.Concrete.ProductImage
                {
                    ImageUrl = imageDto.ImageUrl,
                    IsMain = imageDto.IsMain,
                    ProductId = product.Id
                });
            }
        }

        await productWriteRepository.AddAsync(product, ct);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("product"), ct);

        return Result<CreateProductCommandResponse>.Success(
            new CreateProductCommandResponse(product.Id),
            "Product created successfully.");
    }
}