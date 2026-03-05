using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Enums;
using CheMa.Go.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace CheMa.Go.Applications.AppServices
{
    public class OrderAppService :
        CrudAppService<Order, OrderDto, long, GetListOrderInput, CreateOrderInput, UpdateOrderInput>, IOrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<Passenger, long> _passengerRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public OrderAppService(
            IRepository<Order, long> repository,
            IOrderRepository orderRepository,
            IRepository<Vehicle, long> vehicleRepository,
            IRepository<Passenger, long> passengerRepository,
            IIdentityUserRepository identityUserRepository) : base(repository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _passengerRepository = passengerRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task LinkPassengersToOrderAsync(LinkPassengersToOrderInput input)
        {
            var queryable = await _orderRepository.WithDetailsAsync();
            var order = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Include(x => x.PassengerInfos),
                x => x.Id == input.OrderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), input.OrderId);
            }

            var selectedPassengerIds = input.PassengerIds.Distinct().ToList();
            var passengers = selectedPassengerIds.Count == 0
                ? new List<Passenger>()
                : await _passengerRepository.GetListAsync(x => selectedPassengerIds.Contains(x.Id));

            var removePassengers = order.PassengerInfos
                .Where(x => !selectedPassengerIds.Contains(x.Id))
                .ToList();

            foreach (var removePassenger in removePassengers)
            {
                removePassenger.Status = PassengerStatus.PendingPickup;
                removePassenger.OrderId = null;
                order.PassengerInfos.Remove(removePassenger);
            }

            foreach (var passenger in passengers)
            {
                if (order.PassengerInfos.All(x => x.Id != passenger.Id))
                {
                    order.PassengerInfos.Add(passenger);
                }
            }

            foreach (var passenger in order.PassengerInfos)
            {
                passenger.Status = PassengerStatus.Dispatched;
                passenger.OrderId = order.Id;
            }

            await _orderRepository.UpdateAsync(order, autoSave: true);
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

        public async Task LinkDriverToOrderAsync(LinkDriverToOrderInput input)
        {
            var order = await Repository.GetAsync(input.OrderId);

            if (input.DriverId.HasValue)
            {
                var driver = await _identityUserRepository.FindAsync(input.DriverId.Value);
                if (driver == null)
                {
                    throw new EntityNotFoundException(typeof(IdentityUser), input.DriverId.Value);
                }
            }

            order.DriverId = input.DriverId;
            if (!input.DriverId.HasValue)
            {
                order.Driver = null;
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
                linkedPassenger.Status = PassengerStatus.PendingPickup;
                linkedPassenger.OrderId = null;
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

            if (input.OrderId.HasValue)
            {
                query = query.Where(x => x.Id == input.OrderId.Value);
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
