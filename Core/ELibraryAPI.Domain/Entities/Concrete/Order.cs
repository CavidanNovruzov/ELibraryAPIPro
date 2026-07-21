using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Order : BaseEntity, IOwnership
{
    public string OrderNumber { get; set; } = null!;

    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; } = null!;

    public Guid OrderStatusId { get; set; }
    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public Guid PaymentMethodId { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public Guid ShippingMethodId { get; set; }
    public virtual ShippingMethod ShippingMethod { get; set; } = null!;

    public Guid? ShippingAddressId { get; set; }
    public virtual UserAddress? ShippingAddress { get; set; }

    public string? ShippingAddressLine { get; set; }
    public string? ShippingCity { get; set; }

    public decimal TotalAmount { get; set; }
    public string? OrderNote { get; set; }

    public Guid? PromoCodeId { get; set; } 
    public virtual PromoCode? PromoCode { get; set; }


    public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}