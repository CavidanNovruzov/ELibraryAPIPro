using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class BranchWorkHours : BaseEntity
{
    public Guid BranchId { get; set; }

    public DayOfWeek Day { get; set; }

    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }

    public virtual Branch Branch { get; set; } = null!;

}