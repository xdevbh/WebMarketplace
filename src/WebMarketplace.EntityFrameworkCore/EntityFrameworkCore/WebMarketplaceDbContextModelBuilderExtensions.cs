using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using WebMarketplace.Addresses;
using WebMarketplace.Orders;
using WebMarketplace.Products;
using WebMarketplace.Companies;
using WebMarketplace.Companies.VendorUsers;

namespace WebMarketplace.EntityFrameworkCore;

public static class WebMarketplaceDbContextModelBuilderExtensions
{
    public static void ConfigureWebMarketplace([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.ConfigureAddresses();
        builder.ConfigureCompanies();
        builder.ConfigureProducts();
        builder.ConfigureOrders();
        // builder.ConfigureCarts();
    }
    
    private static void ConfigureAddresses([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Address>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Addresses", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).IsRequired();
            b.Property(x => x.Country).IsRequired();
            b.Property(x => x.State).IsRequired();
            b.Property(x => x.City).IsRequired();
            b.Property(x => x.Line1).IsRequired();
            b.Property(x => x.ZipCode).IsRequired();
            b.Property(x => x.PhoneNumber).IsRequired();
        });
    }
    
    private static void ConfigureCompanies([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Company>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Companies", WebMarketplaceConsts.DbSchema);
            b.Property(x => x.Name).IsRequired();
            b.HasIndex(x=>x.Name).IsUnique();
            b.Property(x => x.DisplayName).IsRequired();
            b.HasIndex(x=>x.DisplayName).IsUnique();
            b.HasOne<Address>().WithMany().HasForeignKey(x => x.AddressId).IsRequired();
            b.ConfigureByConvention();
        });

        builder.Entity<VendorUser>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "VendorUsers", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => new { x.CompanyId, x.UserId }).IsUnique();
        });
    }
    
    private static void ConfigureProducts([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Product>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Products", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); // auto configure for the base class props
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).IsRequired();
            b.HasMany(x => x.ProductReviews).WithOne().IsRequired().HasForeignKey(x => x.ProductId);
            b.HasMany(x => x.ProductPrices).WithOne().IsRequired().HasForeignKey(x => x.ProductId);

        });

        builder.Entity<ProductReview>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "ProductReviews", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); // auto configure for the base class props
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            b.Property(x => x.Rating).IsRequired();
        });
        
        builder.Entity<ProductPrice>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "ProductPrices", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); // auto configure for the base class props
            b.HasKey(x=> new {x.ProductId, x.Date});
            b.Property(x => x.Currency).IsRequired().HasMaxLength(WebMarketplaceConsts.CurrencyCodeLength);
            b.Property(x=>x.Amount).HasColumnType("decimal(18,2)").IsRequired();
        });
    }
    
    private static void ConfigureOrders([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Order>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Orders", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.BuyerId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Address>().WithMany().HasForeignKey(x => x.AddressId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            b.Property(x => x.CompanyName).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
            b.Property(x => x.Currency).IsRequired();
            b.HasMany(x => x.Items).WithOne().IsRequired().HasForeignKey(x => x.OrderId);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "OrderItems", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            b.Property(x => x.ProductName).IsRequired();
            b.Property(x => x.Quantity).IsRequired();
            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
            b.Property(x => x.Currency).IsRequired().HasMaxLength(WebMarketplaceConsts.CurrencyCodeLength);
        });
    }
}