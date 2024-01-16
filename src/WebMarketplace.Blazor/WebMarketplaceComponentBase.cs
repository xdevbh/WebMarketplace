using WebMarketplace.Localization;
using Volo.Abp.AspNetCore.Components;

namespace WebMarketplace.Blazor;

public abstract class WebMarketplaceComponentBase : AbpComponentBase
{
    protected WebMarketplaceComponentBase()
    {
        LocalizationResource = typeof(WebMarketplaceResource);
    }
}
