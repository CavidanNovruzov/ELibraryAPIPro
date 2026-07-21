namespace ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;

public sealed record GetAllPaymentMethodQueryResponse(
    List<PaymentMethodListDto> PaymentMethods,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record PaymentMethodListDto(Guid Id, string Name);
