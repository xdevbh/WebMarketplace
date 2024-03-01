using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using WebMarketplace.Carts;
using WebMarketplace.Currencies;
using WebMarketplace.Orders;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Reviews;
using WebMarketplace.Vendors;

namespace WebMarketplace.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class WebMarketplaceDbContext :
    AbpDbContext<WebMarketplaceDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<UserVendor> UserVendors { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    public WebMarketplaceDbContext(DbContextOptions<WebMarketplaceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(WebMarketplaceConsts.DbTablePrefix + "YourEntities", WebMarketplaceConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        #region Cart_CartItem

        builder.Entity<Cart>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Carts", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });

        builder.Entity<CartItem>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "CartItems", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Cart>().WithMany().HasForeignKey(x => x.CartId).IsRequired();
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
        });

        #endregion
        
        #region Order_OrderItem

        builder.Entity<Order>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Orders", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "OrderItems", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId).IsRequired();
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
        });

        #endregion
        
        #region Product

        builder.Entity<Product>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Products", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasOne<ProductCategory>().WithMany().HasForeignKey(x => x.ProductCategoryId).IsRequired(false);
            b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
        });

        #endregion
        
        #region ProductCategory

        builder.Entity<ProductCategory>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "ProductCategories", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<ProductCategory>().WithMany().HasForeignKey(x=>x.ParentCategoryId).IsRequired(false);
        });

        #endregion
        
        #region ProductCategory

        builder.Entity<ProductCategory>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "ProductCategories", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<ProductCategory>().WithMany().HasForeignKey(x=>x.ParentCategoryId).IsRequired(false);
        });

        #endregion

        #region Vendor

        builder.Entity<Vendor>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Vendors", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
        });

        #endregion

        #region UserVendor

        builder.Entity<UserVendor>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "UserVendors", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
        });

        #endregion

        #region Review

        builder.Entity<Review>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Reviews", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
        });

        #endregion
        
        #region Currency

        builder.Entity<Currency>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Currencies", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).HasMaxLength(3).IsRequired();;
            b.Property(x => x.NumericCode).HasMaxLength(3).IsRequired();
            b.HasIndex(x => x.Code).IsUnique();
            b.HasIndex(x => x.NumericCode).IsUnique();;
        });

        #endregion
    }
}