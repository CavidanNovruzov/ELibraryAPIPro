using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class PaymentMethod : BaseEntity
{
    public string Name { get; set; } = null!;


    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}