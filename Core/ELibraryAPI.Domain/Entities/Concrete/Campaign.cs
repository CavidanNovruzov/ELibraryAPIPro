using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Campaign : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;


    public virtual ICollection<ProductCampaign> ProductCampaigns { get; set; } = new HashSet<ProductCampaign>();
}