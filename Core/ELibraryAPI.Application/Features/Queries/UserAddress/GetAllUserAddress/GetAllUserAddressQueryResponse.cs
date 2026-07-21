namespace ELibraryAPI.Application.Features.Queries.UserAddress.GetAllUserAddress;

public sealed record GetAllUserAddressQueryResponse(
    List<UserAddressListDto> Addresses
);

public sealed record UserAddressListDto(
    Guid Id,
    Guid UserId,
    string AddressLine
);
