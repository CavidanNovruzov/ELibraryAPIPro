using Microsoft.AspNetCore.Identity;

namespace ELibraryAPI.Domain.Entities.Common;

public abstract class IdentityUserBaseEntity<TKey> : IdentityUser<TKey>,
    IEntity<TKey>, IAuditEntity, ISoftDelete
    where TKey : IEquatable<TKey>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}

public abstract class IdentityRoleBaseEntity<TKey> : IdentityRole<TKey>,
    IEntity<TKey>, IAuditEntity, ISoftDelete
    where TKey : IEquatable<TKey>
{
    protected IdentityRoleBaseEntity() : base() { }

    protected IdentityRoleBaseEntity(string roleName) : base(roleName) { }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}