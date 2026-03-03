using System.Threading.Tasks;

namespace CheMa.Go.Data;

public interface IGoDbSchemaMigrator
{
    Task MigrateAsync();
}
