using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch;

public sealed record GetCategorySearchQueryRequest(
  string SearchTerm,
  int Page = 1,
  int Size = 10) : IRequest<Result<GetCategorySearchQueryResponse>>;
