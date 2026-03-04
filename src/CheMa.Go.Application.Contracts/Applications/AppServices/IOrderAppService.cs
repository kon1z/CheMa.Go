using CheMa.Go.Applications.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IOrderAppService : ICrudAppService<OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>
{
    Task LinkVehicleToOrderAsync(LinkVehicleToOrderInput input);

    Task LinkDriverToOrderAsync(LinkDriverToOrderInput input);

    Task RemovePassengerFromOrderAsync(long orderId, long passengerId);
}
