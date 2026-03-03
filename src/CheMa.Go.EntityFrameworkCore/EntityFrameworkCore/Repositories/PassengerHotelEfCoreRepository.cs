using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CheMa.Go.EntityFrameworkCore.Repositories
{
    public class PassengerHotelEfCoreRepository : EfCoreRepository<GoDbContext, Passenger, long>, IPassengerRepository
    {
        public PassengerHotelEfCoreRepository(IDbContextProvider<GoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
