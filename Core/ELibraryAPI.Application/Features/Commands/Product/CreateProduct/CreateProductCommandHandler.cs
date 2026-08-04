using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
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
    private readonly ICurrentUserService _currentUserService;

    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request, CancellationToken ct)
    {
        if (!_currentUserService.IsAdmin)
            return Result<CreateProductCommandResponse>.Failure("Məhsul yaratmaq üçün inzibatçı hüququnuz olmalıdır.", ErrorType.Forbidden);

        var productReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var productWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Product, Guid>();

        // 2. ISBN Unikallıq Yoxlaması (Conflict)
        var isIsbnExists = await productReadRepository.ExistsAsync(
            x => x.ISBN == request.ISBN.Trim(),
            tracking: false,
            ct: ct);

        if (isIsbnExists)
            return Result<CreateProductCommandResponse>.Failure("Bu ISBN nömrəli məhsul artıq mövcuddur.", ErrorType.Conflict);

        var product = _mapper.Map<Domain.Entities.Concrete.Product>(request);

        // 3. Müəlliflərin (Authors) Yoxlanılması
        var authorIds = request.AuthorIds?.Distinct().ToList() ?? new List<Guid>();
        if (authorIds.Any())
        {
            var authorRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>();
            var existingAuthorsCount = await authorRepo
                .GetAll(tracking: false)
                .Where(a => authorIds.Contains(a.Id))
                .CountAsync(ct);

            if (existingAuthorsCount != authorIds.Count)
                return Result<CreateProductCommandResponse>.Failure("Göstərilən müəllif ID-lərindən biri və ya bir neçəsi tapılmadı.", ErrorType.NotFound);

            foreach (var authorId in authorIds)
            {
                product.ProductAuthors.Add(new Domain.Entities.Concrete.ProductAuthor
                {
                    AuthorId = authorId,
                    ProductId = product.Id
                });
            }
        }

        // 4. Janrların (Genres) Yoxlanılması və Əlavə Olunması
        var genreIds = request.GenreIds?.Distinct().ToList() ?? new List<Guid>();
        if (genreIds.Any())
        {
            var genreRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Genre, Guid>();
            var existingGenresCount = await genreRepo
                .GetAll(tracking: false)
                .Where(g => genreIds.Contains(g.Id))
                .CountAsync(ct);

            if (existingGenresCount != genreIds.Count)
                return Result<CreateProductCommandResponse>.Failure("Göstərilən janr ID-lərindən biri və ya bir neçəsi tapılmadı.", ErrorType.NotFound);

            foreach (var genreId in genreIds)
            {
                product.ProductGenres.Add(new Domain.Entities.Concrete.ProductGenre
                {
                    GenreId = genreId,
                    ProductId = product.Id
                });
            }
        }

        // 5. Teqlərin (Tags) Yoxlanılması və Əlavə Olunması
        var tagIds = request.TagIds?.Distinct().ToList() ?? new List<Guid>();
        if (tagIds.Any())
        {
            var tagRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Tag, Guid>();
            var existingTagsCount = await tagRepo
                .GetAll(tracking: false)
                .Where(t => tagIds.Contains(t.Id))
                .CountAsync(ct);

            if (existingTagsCount != tagIds.Count)
                return Result<CreateProductCommandResponse>.Failure("Göstərilən teq ID-lərindən biri və ya bir neçəsi tapılmadı.", ErrorType.NotFound);

            foreach (var tagId in tagIds)
            {
                product.ProductTags.Add(new Domain.Entities.Concrete.ProductTag
                {
                    TagId = tagId,
                    ProductId = product.Id
                });
            }
        }

        // 6. Şəkillərin (Images) Əlavə Edilməsi
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

        await _mediator.Publish(new EntityChangedEvent("product", product.Id), ct);

        return Result<CreateProductCommandResponse>.Success(
            new CreateProductCommandResponse(product.Id),
            "Məhsul uğurla yaradıldı.");
    }
}