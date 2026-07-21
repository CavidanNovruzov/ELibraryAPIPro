using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.CreatePaymentMethod;

public sealed record CreatePaymentMethodCommandRequest(
    string Name
) : IRequest<Result<CreatePaymentMethodCommandResponse>>;
