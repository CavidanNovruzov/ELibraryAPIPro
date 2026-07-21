using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class ProductCampaign : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; } = null!;
}