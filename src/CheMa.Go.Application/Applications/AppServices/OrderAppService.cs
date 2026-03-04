using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices
{
    public class OrderAppService :
        CrudAppService<Order, OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>, IOrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Passenger, long> _passengerRepository;

        public OrderAppService(
            IRepository<Order, long> repository,
            IOrderRepository orderRepository,
            IRepository<Passenger, long> passengerRepository) : base(repository)
        {
            _orderRepository = orderRepository;
            _passengerRepository = passengerRepository;
        }

        public async Task<OrderDto> GetListOrderPassengersAsync(long orderId)
        {
            var queryable = await _orderRepository.WithDetailsAsync();
            var order = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Include(x => x.PassengerInfos).Include(x => x.Vehicle),
                x => x.Id == orderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task LinkPassengersToOrderAsync(LinkPassengersToOrderInput input)
        {
            if (input.PassengerIds.Count == 0)
            {
                return;
            }

            var queryable = await _orderRepository.WithDetailsAsync();
            var order = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Include(x => x.PassengerInfos),
                x => x.Id == input.OrderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), input.OrderId);
            }

            var passengers = await _passengerRepository.GetListAsync(x => input.PassengerIds.Contains(x.Id));
            foreach (var passenger in passengers)
            {
                if (order.PassengerInfos.All(x => x.Id != passenger.Id))
                {
                    order.PassengerInfos.Add(passenger);
                }
            }

            await _orderRepository.UpdateAsync(order, autoSave: true);
        }

        public async Task RemovePassengerFromOrderAsync(long orderId, long passengerId)
        {
            var queryable = await _orderRepository.WithDetailsAsync();
            var order = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Include(x => x.PassengerInfos),
                x => x.Id == orderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            var linkedPassenger = order.PassengerInfos.FirstOrDefault(x => x.Id == passengerId);
            if (linkedPassenger != null)
            {
                order.PassengerInfos.Remove(linkedPassenger);
                await _orderRepository.UpdateAsync(order, autoSave: true);
            }
        }

        protected override async Task<IQueryable<Order>> CreateFilteredQueryAsync(GetListOrderInput input)
        {
            var queryable = await base.CreateFilteredQueryAsync(input);
            IQueryable<Order> query = queryable.Include(x => x.Vehicle).Include(x => x.PassengerInfos);

            if (input.OrderType.HasValue)
            {
                query = query.Where(x => x.OrderType == input.OrderType.Value);
            }

            if (input.OrderStatus.HasValue)
            {
                query = query.Where(x => x.OrderStatus == input.OrderStatus.Value);
            }

            if (input.AppointmentStartTime.HasValue)
            {
                query = query.Where(x => x.AppointmentTime >= input.AppointmentStartTime.Value);
            }

            if (input.AppointmentEndTime.HasValue)
            {
                query = query.Where(x => x.AppointmentTime <= input.AppointmentEndTime.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.LicenseNum))
            {
                query = query.Where(x => x.Vehicle != null && x.Vehicle.LicenseNum.Contains(input.LicenseNum));
            }

            return query;
        }
    }
}
