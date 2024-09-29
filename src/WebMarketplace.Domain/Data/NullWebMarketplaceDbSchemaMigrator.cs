using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WebMarketplace.Data;

/* This is used if database provider does't define
 * IWebMarketplaceDbSchemaMigrator implementation.
 */
public class NullWebMarketplaceDbSchemaMigrator : IWebMarketplaceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
