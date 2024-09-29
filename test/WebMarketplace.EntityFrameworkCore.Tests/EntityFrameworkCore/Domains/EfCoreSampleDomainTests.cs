using WebMarketplace.Samples;
using Xunit;

namespace WebMarketplace.EntityFrameworkCore.Domains;

[Collection(WebMarketplaceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WebMarketplaceEntityFrameworkCoreTestModule>
{

}
