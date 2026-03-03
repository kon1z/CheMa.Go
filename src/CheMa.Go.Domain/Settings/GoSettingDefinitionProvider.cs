using Volo.Abp.Settings;

namespace CheMa.Go.Settings;

public class GoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(GoSettings.MySetting1));
    }
}
