using CheMa.Go.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {
    }
}
