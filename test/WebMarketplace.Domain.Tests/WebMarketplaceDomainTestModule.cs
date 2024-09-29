using Volo.Abp.Modularity;

namespace WebMarketplace;

[DependsOn(
    typeof(WebMarketplaceDomainModule),
    typeof(WebMarketplaceTestBaseModule)
)]
public class WebMarketplaceDomainTestModule : AbpModule
{

}
