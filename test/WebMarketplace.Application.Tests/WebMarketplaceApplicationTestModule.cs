using Volo.Abp.Modularity;

namespace WebMarketplace;

[DependsOn(
    typeof(WebMarketplaceApplicationModule),
    typeof(WebMarketplaceDomainTestModule)
)]
public class WebMarketplaceApplicationTestModule : AbpModule
{

}
