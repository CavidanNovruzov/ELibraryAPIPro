using ELibraryAPI.Domain.Entities.Common;

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


    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}