using ELibraryAPI.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Stock : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid BranchId { get; set; }
    public virtual Branch Branch { get; set; } = null!;

    public int Quantity { get; set; }

    /// <summary>
    /// Concurrency Token — Eyni anda çoxlu update-lərdə Lost Update problemini aradan qaldırır.
    /// Black Friday ssenarisini: iki user eyni anda stok azaltmaq istəyəndə biri DbUpdateConcurrencyException atar.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    /// <summary>
    /// Stokdan miqdar azaldır. Mənfi stok yaranmasının qarşısını invariant kimi burada alır —
    /// handler-lərdə hər dəfə "kifayət qədərdirmi" yoxlamasını təkrarlamağa ehtiyac qalmır.
    /// </summary>
    public void Decrease(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Azaldılan miqdar 0-dan böyük olmalıdır.");

        if (Quantity < amount)
            throw new InvalidOperationException("Stok kifayət qədər deyil.");

        Quantity -= amount;
    }
}
