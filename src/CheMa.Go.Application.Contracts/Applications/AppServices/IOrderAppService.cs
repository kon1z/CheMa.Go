using CheMa.Go.Applications.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IOrderAppService : ICrudAppService<OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>
{
    Task<OrderDto> GetListOrderPassengersAsync(long orderId);

    Task LinkPassengersToOrderAsync(LinkPassengersToOrderInput input);

    Task RemovePassengerFromOrderAsync(long orderId, long passengerId);
}
