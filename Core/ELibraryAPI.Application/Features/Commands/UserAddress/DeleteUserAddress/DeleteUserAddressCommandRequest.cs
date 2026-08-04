using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.DeleteUserAddress;

public sealed record DeleteUserAddressCommandRequest(Guid Id) : IRequest<Result>;
