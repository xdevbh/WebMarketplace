using WebMarketplace.Localization;
using Volo.Abp.AspNetCore.Components;

namespace WebMarketplace.Blazor.Client;

public abstract class WebMarketplaceComponentBase : AbpComponentBase
{
    protected WebMarketplaceComponentBase()
    {
        LocalizationResource = typeof(WebMarketplaceResource);
    }
}
