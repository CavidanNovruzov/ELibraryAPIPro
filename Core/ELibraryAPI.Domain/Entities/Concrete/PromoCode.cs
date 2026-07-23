using ELibraryAPI.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class PromoCode : BaseEntity
{
    public string Code { get; set; } = null!; 
    public decimal DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int UsageLimit { get; set; } 
    public int UsageCount { get; set; } = 0; 

    public bool IsActive { get; set; } = true;


    /// <summary>
    /// Concurrency Token — Stock entity-dəki eyni prinsip. Promo kod son istifadə haqqı
    /// qalanda paralel iki sifariş eyni anda UsageCount-u artırmaq istəyəndə
    /// biri DbUpdateConcurrencyException atmalıdır ki, limitdən artıq istifadə olunmasın.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}