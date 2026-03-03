using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CheMa.Go.EntityFrameworkCore.Repositories
{
    public class VehicleEfCoreRepository : EfCoreRepository<GoDbContext, Vehicle, long>, IVehicleRepository
    {
        public VehicleEfCoreRepository(IDbContextProvider<GoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
