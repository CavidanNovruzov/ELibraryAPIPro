
using ELibraryAPI.Application.Responses;
using MediatR;



namespace ELibraryAPI.Application.Features.Queries.Wishlist;

public sealed record GetCustomerWishlistQueryRequest : IRequest<Result<GetCustomerWishlistQueryResponse>>;
