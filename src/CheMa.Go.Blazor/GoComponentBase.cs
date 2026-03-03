using CheMa.Go.Localization;
using Volo.Abp.AspNetCore.Components;

namespace CheMa.Go.Blazor;

public abstract class GoComponentBase : AbpComponentBase
{
    protected GoComponentBase()
    {
        LocalizationResource = typeof(GoResource);
    }
}
