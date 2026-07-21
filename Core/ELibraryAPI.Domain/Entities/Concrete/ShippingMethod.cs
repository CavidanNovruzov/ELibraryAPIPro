using ELibraryAPI.Domain.Entities.Common;


namespace ELibraryAPI.Domain.Entities.Concrete;

public class ShippingMethod : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}