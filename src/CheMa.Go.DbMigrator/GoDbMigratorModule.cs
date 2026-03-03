using CheMa.Go.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CheMa.Go.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(GoEntityFrameworkCoreModule),
    typeof(GoApplicationContractsModule)
)]
public class GoDbMigratorModule : AbpModule
{
}
