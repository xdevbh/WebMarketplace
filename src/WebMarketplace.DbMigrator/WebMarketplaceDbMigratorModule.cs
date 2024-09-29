using WebMarketplace.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace WebMarketplace.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WebMarketplaceEntityFrameworkCoreModule),
    typeof(WebMarketplaceApplicationContractsModule)
)]
public class WebMarketplaceDbMigratorModule : AbpModule
{
}
