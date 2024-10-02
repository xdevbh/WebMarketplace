using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using WebMarketplace.Addresses;
using WebMarketplace.Carts;
using WebMarketplace.Orders;
using WebMarketplace.Products;
using WebMarketplace.Vendors;
using WebMarketplace.Vendors.VendorUsers;

namespace WebMarketplace.EntityFrameworkCore;

public static class WebMarketplaceDbContextModelBuilderExtensions
{
    public static void ConfigureWebMarketplace([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.ConfigureAddresses();
        builder.ConfigureVendors();
        builder.ConfigureProducts();
        // builder.ConfigureCarts();
        // builder.ConfigureOrders();
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
    
    private static void ConfigureVendors([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Vendor>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Vendors", WebMarketplaceConsts.DbSchema);
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
            b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => new { x.VendorId, x.UserId }).IsUnique();
        });
    }
    
    private static void ConfigureProducts([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Product>(b =>
        {
            b.ToTable(WebMarketplaceConsts.DbTablePrefix + "Products", WebMarketplaceConsts.DbSchema);
            b.ConfigureByConvention(); // auto configure for the base class props
            b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
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

    private static void ConfigureCarts([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

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
    }

    private static void ConfigureOrders([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

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
    }
}