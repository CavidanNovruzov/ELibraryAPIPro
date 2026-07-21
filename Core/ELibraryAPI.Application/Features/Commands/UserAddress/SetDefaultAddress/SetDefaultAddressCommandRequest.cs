using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.SetDefaultAddress;

public sealed record SetDefaultAddressCommandRequest(Guid Id, Guid UserId) : IRequest<Result>;