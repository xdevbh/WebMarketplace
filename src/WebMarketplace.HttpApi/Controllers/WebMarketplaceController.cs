using WebMarketplace.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WebMarketplace.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WebMarketplaceController : AbpControllerBase
{
    protected WebMarketplaceController()
    {
        LocalizationResource = typeof(WebMarketplaceResource);
    }
}
