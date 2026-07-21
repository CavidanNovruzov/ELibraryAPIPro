namespace ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;

public sealed record GetAllShippingMethodQueryResponse(
    List<ShippingMethodListDto> ShippingMethods,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record ShippingMethodListDto(Guid Id, string Name, decimal Price);
