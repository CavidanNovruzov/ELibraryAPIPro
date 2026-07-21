using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch
{
    public sealed class GetCategorySearchQueryHandler : IRequestHandler<GetCategorySearchQueryRequest, Result<GetCategorySearchQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategorySearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetCategorySearchQueryResponse>> Handle(GetCategorySearchQueryRequest request, CancellationToken ct)
        {
            var searchTerm = request.SearchTerm.ToLower();

            var categoriesQuery = _unitOfWork
                .ReadRepository<Domain.Entities.Concrete.Category, Guid>()
                .GetWhere(c => c.Name.ToLower().Contains(searchTerm), tracking: false)
                .Select(c => new CategorySearchDto(c.Id, c.Name, "Main Category"));

            var subCategoriesQuery = _unitOfWork
                .ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>()
                .GetWhere(sc => sc.Name.ToLower().Contains(searchTerm), tracking: false)
                .Select(sc => new CategorySearchDto(sc.Id, sc.Name, "Sub-Category"));

            IQueryable<CategorySearchDto> combinedQuery = categoriesQuery.Concat(subCategoriesQuery);

            var totalCount = await combinedQuery.CountAsync(ct);

            var results = await combinedQuery
                .OrderBy(x => x.Name)
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync(ct);

            return Result<GetCategorySearchQueryResponse>.Success(
                new GetCategorySearchQueryResponse(results, totalCount)
            );
        }
    }
}
