using Volo.Abp.Modularity;

namespace WebMarketplace;

/* Inherit from this class for your domain layer tests. */
public abstract class WebMarketplaceDomainTestBase<TStartupModule> : WebMarketplaceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
