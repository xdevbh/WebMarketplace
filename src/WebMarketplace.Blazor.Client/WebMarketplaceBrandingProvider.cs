using Microsoft.Extensions.Localization;
using WebMarketplace.Localization;
using Microsoft.Extensions.Localization;
using WebMarketplace.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WebMarketplace.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class WebMarketplaceBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebMarketplaceResource> _localizer;

    public WebMarketplaceBrandingProvider(IStringLocalizer<WebMarketplaceResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
