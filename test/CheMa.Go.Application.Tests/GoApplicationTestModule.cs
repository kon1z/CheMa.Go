using Volo.Abp.Modularity;

namespace CheMa.Go;

[DependsOn(
    typeof(GoApplicationModule),
    typeof(GoDomainTestModule)
)]
public class GoApplicationTestModule : AbpModule
{

}
