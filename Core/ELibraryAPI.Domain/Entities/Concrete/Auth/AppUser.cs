using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete.Auth;


public class AppUser : IdentityUserBaseEntity<Guid>
{
    public AppUser() : base()
    {
        RefreshTokens      = new HashSet<RefreshToken>();
        Orders             = new HashSet<Order>();
        Reviews            = new HashSet<Review>();
        Addresses          = new HashSet<UserAddress>();
        SearchHistories    = new HashSet<UserSearchHistory>();
        UserPermissions    = new HashSet<AppUserPermission>();
        GrantedPermissions = new HashSet<AppUserPermission>();
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public virtual Basket?   Basket   { get; set; }
    public virtual Wishlist? Wishlist { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens  { get; set; }
    public virtual ICollection<Order>   Orders { get; set; }
    public virtual ICollection<Review>  Reviews  { get; set; }
    public virtual ICollection<UserAddress> Addresses { get; set; }
    public virtual ICollection<UserSearchHistory>  SearchHistories  { get; set; }
    public virtual ICollection<AppUserPermission>  UserPermissions  { get; set; }
    public virtual ICollection<AppUserPermission>  GrantedPermissions { get; set; }
}

