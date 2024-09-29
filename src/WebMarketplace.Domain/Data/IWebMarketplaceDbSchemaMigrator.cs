using System.Threading.Tasks;

namespace WebMarketplace.Data;

public interface IWebMarketplaceDbSchemaMigrator
{
    Task MigrateAsync();
}
