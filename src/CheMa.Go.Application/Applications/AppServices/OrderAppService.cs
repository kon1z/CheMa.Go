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
        private readonly IRepository<Vehicle, long> _vehicleRepository;

        public OrderAppService(
            IRepository<Order, long> repository,
            IOrderRepository orderRepository,
            IRepository<Vehicle, long> vehicleRepository) : base(repository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task LinkVehicleToOrderAsync(LinkVehicleToOrderInput input)
        {
            var order = await Repository.GetAsync(input.OrderId);

            if (input.VehicleId.HasValue)
            {
                var hasVehicle = await _vehicleRepository.AnyAsync(x => x.Id == input.VehicleId.Value);
                if (!hasVehicle)
                {
                    throw new EntityNotFoundException(typeof(Vehicle), input.VehicleId.Value);
                }
            }

            order.VehicleId = input.VehicleId;
            if (!input.VehicleId.HasValue)
            {
                order.Vehicle = null;
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
            IQueryable<Order> query = queryable.Include(x => x.Vehicle)
                .Include(x => x.PassengerInfos)
                .Include(x => x.Driver);

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
