using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Transaction : BaseEntity, ISoftDelete
{
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;

    public string Currency { get; set; } = "AZN";

    public string PaymentProvider { get; set; } = null!;

    public decimal Amount { get; set; }

    public string TransactionId { get; set; } = null!;

    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    [NotMapped]
    public bool IsSuccess => Status == TransactionStatus.Success;

    public string? ProviderResponse { get; set; }

}
