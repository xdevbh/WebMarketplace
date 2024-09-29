using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMarketplace.Data;
using Volo.Abp.DependencyInjection;

namespace WebMarketplace.EntityFrameworkCore;

public class EntityFrameworkCoreWebMarketplaceDbSchemaMigrator
    : IWebMarketplaceDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreWebMarketplaceDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the WebMarketplaceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebMarketplaceDbContext>()
            .Database
            .MigrateAsync();
    }
}
