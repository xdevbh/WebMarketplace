using Volo.Abp.Modularity;

namespace WebMarketplace;

public abstract class WebMarketplaceApplicationTestBase<TStartupModule> : WebMarketplaceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
