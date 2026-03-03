using CheMa.Go.Localization;
using Volo.Abp.Application.Services;

namespace CheMa.Go;

/* Inherit your application services from this class.
 */
public abstract class GoAppService : ApplicationService
{
    protected GoAppService()
    {
        LocalizationResource = typeof(GoResource);
    }
}
