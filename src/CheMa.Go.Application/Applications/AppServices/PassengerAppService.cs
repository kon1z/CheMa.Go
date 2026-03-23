using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices
{
    public class PassengerAppService :
        CrudAppService<Passenger, PassengerDto, long, GetListPassengerInput, CreatePassengerInput,
            UpdatePassengerInput>, IPassengerAppService
    {
        private readonly IRepository<Order, long> _orderRepository;

        public PassengerAppService(IRepository<Passenger, long> repository, IRepository<Order, long> orderRepository) : base(repository)
        {
            _orderRepository = orderRepository;
        }

        public async Task SetBoardedAsync(long id)
        {
            var passenger = await Repository.GetAsync(id);
            passenger.Status = PassengerStatus.Boarded;
            await Repository.UpdateAsync(passenger, autoSave: true);
            await RefreshOrderStatusAsync(passenger.OrderId);
        }

        public async Task SetPassengerExitAsync(long id)
        {
            var passenger = await Repository.GetAsync(id);
            passenger.Status = PassengerStatus.Completed;
            await Repository.UpdateAsync(passenger, autoSave: true);
            await RefreshOrderStatusAsync(passenger.OrderId);
        }

        public async Task SetExceptionClosedAsync(long id)
        {
            var passenger = await Repository.GetAsync(id);
            passenger.Status = PassengerStatus.ExceptionClosed;
            await Repository.UpdateAsync(passenger, autoSave: true);
            await RefreshOrderStatusAsync(passenger.OrderId);
        }

        protected override async Task<IQueryable<Passenger>> CreateFilteredQueryAsync(GetListPassengerInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                query = query.Where(x => x.Name.Contains(input.Name));
            }

            if (!string.IsNullOrWhiteSpace(input.Phone))
            {
                query = query.Where(x => x.Phone.Contains(input.Phone));
            }

            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status.Value);
            }

            if (input.OrderId.HasValue)
            {
                query = query.Where(x => x.OrderId == input.OrderId.Value);
            }

            return query;
        }

        private async Task RefreshOrderStatusAsync(long? orderId)
        {
            if (!orderId.HasValue)
            {
                return;
            }

            var query = await _orderRepository.WithDetailsAsync(x => x.PassengerInfos);
            var order = query.FirstOrDefault(x => x.Id == orderId.Value);
            if (order == null)
            {
                return;
            }

            if (order.OrderStatus != OrderStatus.Arrived && order.OrderStatus != OrderStatus.PartiallyPicked && order.OrderStatus != OrderStatus.Picked)
            {
                return;
            }

            var boardedCount = order.PassengerInfos.Count(x => x.Status == PassengerStatus.Boarded);
            if (boardedCount == 0)
            {
                order.OrderStatus = OrderStatus.Arrived;
            }
            else if (boardedCount == order.PassengerInfos.Count)
            {
                order.OrderStatus = OrderStatus.Picked;
            }
            else
            {
                order.OrderStatus = OrderStatus.PartiallyPicked;
            }

            await _orderRepository.UpdateAsync(order, autoSave: true);
        }
    }
}
