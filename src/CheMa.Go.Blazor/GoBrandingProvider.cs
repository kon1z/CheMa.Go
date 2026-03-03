using Microsoft.Extensions.Localization;
using CheMa.Go.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace CheMa.Go.Blazor;

[Dependency(ReplaceServices = true)]
public class GoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<GoResource> _localizer;

    public GoBrandingProvider(IStringLocalizer<GoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
