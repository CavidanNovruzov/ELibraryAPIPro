using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Order.GetOrdersByStatus;

public sealed record GetOrdersByStatusQueryRequest(Guid StatusId) : IRequest<Result<GetOrdersByStatusQueryResponse>>;
