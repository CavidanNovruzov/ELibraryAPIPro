using ELibraryAPI.Domain.Entities.Common;


namespace ELibraryAPI.Domain.Entities.Concrete;

public class WishlistItem : BaseEntity
{
    public Guid WishlistId { get; set; }
    public virtual Wishlist Wishlist { get; set; } = null!;

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public bool NotifyWhenAvailable { get; set; } = false;
}