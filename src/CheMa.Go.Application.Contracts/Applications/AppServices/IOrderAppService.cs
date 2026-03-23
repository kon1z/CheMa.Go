using CheMa.Go.Applications.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IOrderAppService : ICrudAppService<OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>
{
    Task LinkPassengersToOrderAsync(LinkPassengersToOrderInput input);

    Task LinkVehicleToOrderAsync(LinkVehicleToOrderInput input);

    Task LinkDriverToOrderAsync(LinkDriverToOrderInput input);

    Task<DispatchConflictCheckResultDto> CheckDispatchConflictsAsync(long orderId);

    Task ConfirmDispatchAsync(long orderId);

    Task StartTripAsync(long orderId);

    Task ArriveAsync(long orderId);

    Task RejectDispatchAsync(long orderId);

    Task ReturnToPendingAsync(long orderId);

    Task CompleteOrderAsync(long orderId);

    Task ForceTransferPassengersAsync(ForceTransferPassengerInput input);

    Task RemovePassengerFromOrderAsync(long orderId, long passengerId);
}
