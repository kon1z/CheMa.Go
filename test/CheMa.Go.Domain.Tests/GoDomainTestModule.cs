using Volo.Abp.Modularity;

namespace CheMa.Go;

[DependsOn(
    typeof(GoDomainModule),
    typeof(GoTestBaseModule)
)]
public class GoDomainTestModule : AbpModule
{

}
