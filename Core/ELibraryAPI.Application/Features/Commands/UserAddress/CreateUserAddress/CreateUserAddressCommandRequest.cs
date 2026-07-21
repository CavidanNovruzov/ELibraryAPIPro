using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;

public sealed record CreateUserAddressCommandRequest(
    string AddressLine,
    Guid UserId,
    bool IsDefault
) : IRequest<Result<CreateUserAddressCommandResponse>>;
