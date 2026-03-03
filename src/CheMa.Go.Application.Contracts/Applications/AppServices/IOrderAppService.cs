using CheMa.Go.Applications.Dtos;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IOrderAppService : ICrudAppService<OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>
{
}