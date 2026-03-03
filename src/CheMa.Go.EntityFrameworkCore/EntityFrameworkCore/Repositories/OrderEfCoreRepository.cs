using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CheMa.Go.EntityFrameworkCore.Repositories
{
    public class OrderEfCoreRepository : EfCoreRepository<GoDbContext, Order, long>, IOrderRepository
    {
        public OrderEfCoreRepository(IDbContextProvider<GoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
