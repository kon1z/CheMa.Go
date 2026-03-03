using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CheMa.Go.Data;

/* This is used if database provider does't define
 * IGoDbSchemaMigrator implementation.
 */
public class NullGoDbSchemaMigrator : IGoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
