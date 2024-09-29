using Xunit;

namespace WebMarketplace.EntityFrameworkCore;

[CollectionDefinition(WebMarketplaceTestConsts.CollectionDefinitionName)]
public class WebMarketplaceEntityFrameworkCoreCollection : ICollectionFixture<WebMarketplaceEntityFrameworkCoreFixture>
{

}
