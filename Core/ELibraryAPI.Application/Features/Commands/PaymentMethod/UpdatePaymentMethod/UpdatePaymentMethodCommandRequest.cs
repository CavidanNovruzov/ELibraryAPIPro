using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;

public sealed record UpdatePaymentMethodCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdatePaymentMethodCommandResponse>>;
