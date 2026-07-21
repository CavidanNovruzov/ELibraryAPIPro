namespace ELibraryAPI.Domain.Constants;

public static class AuthorizePermissions
{
    // Kitab və Məhsul idarəetməsi
    public static class Books
    {
        public const string View = "Permissions.Books.View";
        public const string Create = "Permissions.Books.Create";
        public const string Edit = "Permissions.Books.Edit";
        public const string Delete = "Permissions.Books.Delete";
    }

    // Kataloq Metadataları (Kategoriya, Janr, Nəşriyyat, Dil və s.)
    public static class Catalog
    {
        public const string ManageCategories = "Permissions.Catalog.ManageCategories";
        public const string ManageGenres = "Permissions.Catalog.ManageGenres";
        public const string ManagePublishers = "Permissions.Catalog.ManagePublishers";
        public const string ManageTags = "Permissions.Catalog.ManageTags";
        public const string ManageLanguages = "Permissions.Catalog.ManageLanguages";
        public const string ManageCoverTypes = "Permissions.Catalog.ManageCoverTypes";
    }

    // İnventar və Stok idarəetməsi
    public static class Inventory
    {
       
        public const string ViewStock = "Permissions.Inventory.ViewStock";
        public const string ManageStock = "Permissions.Inventory.ManageStock";
        public const string ViewMovements = "Permissions.Inventory.ViewMovements";
    }

    // Müəllif idarəetməsi
    public static class Authors
    {
        public const string View = "Permissions.Authors.View";
        public const string Create = "Permissions.Authors.Create";
        public const string Edit = "Permissions.Authors.Edit";
        public const string Delete = "Permissions.Authors.Delete";
    }

    // Sifariş və Satış idarəetməsi
    public static class Orders
    {
        public const string View = "Permissions.Orders.View";
        public const string UpdateStatus = "Permissions.Orders.UpdateStatus";
        public const string Cancel = "Permissions.Orders.Cancel";
        public const string ManageShippingMethods = "Permissions.Orders.ManageShippingMethods";
    }

    // Maliyyə və Ödənişlər
    public static class Finance
    {
        public const string ViewTransactions = "Permissions.Finance.ViewTransactions";
        public const string ViewPriceHistory = "Permissions.Finance.ViewPriceHistory";
        public const string ManagePaymentMethods = "Permissions.Finance.ManagePaymentMethods";
    }

    // Müştəri Rəyləri (Review Moderasiyası)
    public static class Reviews
    {
        public const string View = "Permissions.Reviews.View";
        public const string Moderate = "Permissions.Reviews.Moderate"; // Rəyləri təsdiqləmək və ya silmək
    }

    // Kampaniyalar, Bannerlər və Promo kodlar
    public static class Marketing
    {
        public const string ManageBanners = "Permissions.Marketing.ManageBanners";
        public const string ManageCampaigns = "Permissions.Marketing.ManageCampaigns";
        public const string ManagePromoCodes = "Permissions.Marketing.ManagePromoCodes";
    }

    // Səbət əməliyyatları (Admin tərəfi)
    public static class Basket
    {
        public const string ViewAll = "Permissions.Basket.ViewAll";
        public const string ExportData = "Permissions.Basket.ExportData";
    }

    // Filial və İş saatları
    public static class Branches // Controller "Branch" (tək) gözləyir
    {
        public const string View = "Permissions.Branch.View";
        public const string Create = "Permissions.Branch.Create";
        public const string Update = "Permissions.Branch.Update"; // Həm Update, həm ChangeStatus üçün
        public const string Delete = "Permissions.Branch.Delete";
    }

    // Sistem və İstifadəçi idarəetməsi
    public static class Administration
    {
        public const string ViewLogs = "Permissions.Administration.ViewLogs";
        public const string ManageRoles = "Permissions.Administration.ManageRoles";
        public const string ManagePermissions = "Permissions.Administration.ManagePermissions";
        public const string AssignPermissions = "Permissions.Administration.AssignPermissions";
        public const string ManageUsers = "Permissions.Administration.ManageUsers";
    }
}