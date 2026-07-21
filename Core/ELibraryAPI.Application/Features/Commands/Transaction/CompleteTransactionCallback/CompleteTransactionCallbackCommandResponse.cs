
namespace ELibraryAPI.Application.Features.Commands.Transaction.CompleteTransactionCallback;

public sealed record CompleteTransactionCallbackCommandResponse(Guid OrderId, bool StatusUpdated);
