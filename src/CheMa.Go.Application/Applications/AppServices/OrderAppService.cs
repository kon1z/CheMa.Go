using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices
{
    public class OrderAppService :
        CrudAppService<Order, OrderDto, long, GetListOrderInput, CreateOrderInput,
            UpdateOrderInput>, IOrderAppService
    {
        public OrderAppService(IRepository<Order, long> repository) : base(repository)
        {
        }
    }
}
