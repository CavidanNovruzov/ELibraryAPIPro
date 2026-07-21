using System.ComponentModel.DataAnnotations.Schema;
using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Basket : BaseEntity, IOwnership
{
    public Basket()
    {
        BasketItems = new HashSet<BasketItem>();
    }

    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; } = null!;

    public virtual ICollection<BasketItem> BasketItems { get; set; }

    [NotMapped]
    public decimal TotalPrice
        => BasketItems?.Sum(x =>
            (x.Product?.DiscountPrice ?? x.Product?.SalePrice ?? 0)
            * x.Quantity
        ) ?? 0;
}