using Volo.Abp.Settings;

namespace WebMarketplace.Settings;

public class WebMarketplaceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WebMarketplaceSettings.MySetting1));
    }
}
