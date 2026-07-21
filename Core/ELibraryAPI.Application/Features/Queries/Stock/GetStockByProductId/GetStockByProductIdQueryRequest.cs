using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;

public sealed record GetStockByProductIdQueryRequest(Guid ProductId) : IRequest<Result<GetStockByProductIdQueryResponse>>;
