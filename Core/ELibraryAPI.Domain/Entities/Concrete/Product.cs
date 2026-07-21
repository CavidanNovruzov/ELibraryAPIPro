﻿using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Product : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int PageCount { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int PublicationYear { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid PublisherId { get; set; }
    public virtual Publisher Publisher { get; set; } = null!;

    public Guid LanguageId { get; set; }
    public virtual Language Language { get; set; } = null!;

    public Guid CoverTypeId { get; set; }
    public virtual CoverType CoverType { get; set; } = null!;

    public Guid SubCategoryId { get; set; }
    public virtual SubCategory SubCategory { get; set; } = null!;

    public virtual ICollection<ProductAuthor> ProductAuthors { get; set; } = new HashSet<ProductAuthor>();
    public virtual ICollection<ProductGenre> ProductGenres { get; set; } = new HashSet<ProductGenre>();
    public virtual ICollection<ProductTag> ProductTags { get; set; } = new HashSet<ProductTag>();
    public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();
    public virtual ICollection<ProductCampaign> ProductCampaigns { get; set; } = new HashSet<ProductCampaign>();

    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new HashSet<WishlistItem>();

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new HashSet<BasketItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    public virtual ICollection<Stock> Stocks { get; set; } = new HashSet<Stock>();
    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new HashSet<PriceHistory>();
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new HashSet<InventoryMovement>();
}