using Volo.Abp.Modularity;

namespace CheMa.Go;

public abstract class GoApplicationTestBase<TStartupModule> : GoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
