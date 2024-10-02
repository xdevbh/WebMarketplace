using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebMarketplace.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class WebMarketplaceDbContextFactory : IDesignTimeDbContextFactory<WebMarketplaceDbContext>
{
    public WebMarketplaceDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        WebMarketplaceEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<WebMarketplaceDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new WebMarketplaceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebMarketplace.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.secrets.json", optional: false);

        return builder.Build();
    }
}
