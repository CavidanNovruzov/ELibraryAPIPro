using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.DeletePaymentMethod;

public sealed record DeletePaymentMethodCommandRequest(Guid Id) : IRequest<Result>;
