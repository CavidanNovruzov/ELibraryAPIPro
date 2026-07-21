using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Basket.GetMyBasket;

public sealed record GetMyBasketQueryRequest() : IRequest<Result<GetMyBasketQueryResponse>>;
