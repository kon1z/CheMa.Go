using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Enums;
using CheMa.Go.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CheMa.Go.EntityFrameworkCore.Repositories
{
    public class EfCoreOrderRepository : EfCoreRepository<GoDbContext, Order, long>, IOrderRepository
    {
        public EfCoreOrderRepository(IDbContextProvider<GoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Order>> GetConflictingOrdersAsync(long orderId, List<long> passengerIds)
        {
            var dbSet = await GetDbSetAsync();
            var occupiedStatuses = new[]
            {
                OrderStatus.Pending,
                OrderStatus.Dispatched,
                OrderStatus.IsOnTheWay,
                OrderStatus.Arrived,
                OrderStatus.Picked
            };

            return await dbSet
                .Include(x => x.Vehicle)
                .Include(x => x.Driver)
                .Include(x => x.PassengerInfos)
                .Where(x => x.Id != orderId && occupiedStatuses.Contains(x.OrderStatus) && x.PassengerInfos.Any(p => passengerIds.Contains(p.Id)))
                .ToListAsync();
        }
    }
}
