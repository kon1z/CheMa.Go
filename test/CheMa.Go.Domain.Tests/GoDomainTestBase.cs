using Volo.Abp.Modularity;

namespace CheMa.Go;

/* Inherit from this class for your domain layer tests. */
public abstract class GoDomainTestBase<TStartupModule> : GoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
