

namespace ELibraryAPI.Domain.Entities.Common;

public interface IAuditEntity
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}
