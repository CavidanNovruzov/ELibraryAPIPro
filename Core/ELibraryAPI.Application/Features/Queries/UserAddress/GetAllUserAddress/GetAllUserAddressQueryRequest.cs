using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.UserAddress.GetAllUserAddress;

public sealed record GetAllUserAddressQueryRequest : IRequest<Result<GetAllUserAddressQueryResponse>>;
