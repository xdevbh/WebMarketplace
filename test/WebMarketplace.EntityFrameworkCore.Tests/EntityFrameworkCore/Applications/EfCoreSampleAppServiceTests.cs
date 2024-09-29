using WebMarketplace.Samples;
using Xunit;

namespace WebMarketplace.EntityFrameworkCore.Applications;

[Collection(WebMarketplaceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WebMarketplaceEntityFrameworkCoreTestModule>
{

}
