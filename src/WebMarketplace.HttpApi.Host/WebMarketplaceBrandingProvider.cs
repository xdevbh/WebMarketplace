using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WebMarketplace;

[Dependency(ReplaceServices = true)]
public class WebMarketplaceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "WebMarketplace";
}
