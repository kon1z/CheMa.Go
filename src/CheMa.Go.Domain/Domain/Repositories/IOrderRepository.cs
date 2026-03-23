using CheMa.Go.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {
        Task<List<Order>> GetConflictingOrdersAsync(long orderId, List<long> passengerIds);
    }
}
