using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.DeletePromoCode;

public sealed record DeletePromoCodeCommandRequest(Guid Id) : IRequest<Result>;
