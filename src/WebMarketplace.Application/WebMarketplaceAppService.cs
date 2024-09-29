using WebMarketplace.Localization;
using Volo.Abp.Application.Services;

namespace WebMarketplace;

/* Inherit your application services from this class.
 */
public abstract class WebMarketplaceAppService : ApplicationService
{
    protected WebMarketplaceAppService()
    {
        LocalizationResource = typeof(WebMarketplaceResource);
    }
}
