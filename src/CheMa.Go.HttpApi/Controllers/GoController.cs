using CheMa.Go.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CheMa.Go.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class GoController : AbpControllerBase
{
    protected GoController()
    {
        LocalizationResource = typeof(GoResource);
    }
}
