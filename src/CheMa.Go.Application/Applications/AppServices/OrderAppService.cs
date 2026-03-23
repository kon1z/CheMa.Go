using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Enums;
using CheMa.Go.Domain.Repositories;
using CheMa.Go.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
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
        private readonly IRepository<DispatchLog, long> _dispatchLogRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public OrderAppService(
            IRepository<Order, long> repository,
            IOrderRepository orderRepository,
            IRepository<Vehicle, long> vehicleRepository,
            IRepository<Passenger, long> passengerRepository,
            IRepository<DispatchLog, long> dispatchLogRepository,
            IIdentityUserRepository identityUserRepository) : base(repository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _passengerRepository = passengerRepository;
            _dispatchLogRepository = dispatchLogRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task LinkPassengersToOrderAsync(LinkPassengersToOrderInput input)
        {
            var order = await GetOrderWithPassengersAsync(input.OrderId);
            var selectedPassengerIds = input.PassengerIds.Distinct().ToList();
            var passengers = selectedPassengerIds.Count == 0
                ? new List<Passenger>()
                : await _passengerRepository.GetListAsync(x => selectedPassengerIds.Contains(x.Id));

            var removePassengers = order.PassengerInfos.Where(x => !selectedPassengerIds.Contains(x.Id)).ToList();
            foreach (var removePassenger in removePassengers)
            {
                removePassenger.Status = PassengerStatus.PendingPickup;
                removePassenger.OrderId = null;
                order.PassengerInfos.Remove(removePassenger);
            }

            foreach (var passenger in passengers)
            {
                if (passenger.OrderId.HasValue && passenger.OrderId != order.Id)
                {
                    continue;
                }

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

        public async Task<DispatchConflictCheckResultDto> CheckDispatchConflictsAsync(long orderId)
        {
            var order = await GetOrderWithPassengersAsync(orderId);
            var passengerIds = order.PassengerInfos.Select(x => x.Id).ToList();
            if (passengerIds.Count == 0)
            {
                return new DispatchConflictCheckResultDto();
            }

            var conflictingOrders = await _orderRepository.GetConflictingOrdersAsync(orderId, passengerIds);
            var conflicts = conflictingOrders
                .SelectMany(orderItem => orderItem.PassengerInfos
                    .Where(passenger => passengerIds.Contains(passenger.Id))
                    .Select(passenger => new DispatchConflictItemDto
                    {
                        PassengerId = passenger.Id,
                        PassengerName = passenger.Name,
                        OrderId = orderItem.Id,
                        OrderStatus = orderItem.OrderStatus,
                        AppointmentTime = orderItem.AppointmentTime,
                        DriverName = orderItem.Driver?.UserName,
                        VehicleLicenseNum = orderItem.Vehicle?.LicenseNum
                    }))
                .OrderBy(x => x.PassengerId)
                .ToList();

            return new DispatchConflictCheckResultDto { Conflicts = conflicts };
        }

        [Authorize(GoPermissions.Orders.ConfirmDispatch)]
        public async Task ConfirmDispatchAsync(long orderId)
        {
            var conflictResult = await CheckDispatchConflictsAsync(orderId);
            if (conflictResult.HasConflict)
            {
                throw new UserFriendlyException(L["Order:DispatchConflict"]);
            }

            var order = await Repository.GetAsync(orderId);
            order.OrderStatus = OrderStatus.Dispatched;
            await _orderRepository.UpdateAsync(order, autoSave: true);
        }

        [Authorize(GoPermissions.Orders.ForceTransfer)]
        public async Task ForceTransferPassengersAsync(ForceTransferPassengerInput input)
        {
            if (input.PassengerIds.Count == 0)
            {
                return;
            }

            var targetOrder = await GetOrderWithPassengersAsync(input.TargetOrderId);
            if (targetOrder.OrderStatus != OrderStatus.Pending && targetOrder.OrderStatus != OrderStatus.Dispatched)
            {
                throw new UserFriendlyException(L["Order:TransferTargetInvalid"]);
            }

            var passengers = await _passengerRepository.GetListAsync(x => input.PassengerIds.Contains(x.Id));
            foreach (var passenger in passengers)
            {
                if (!passenger.OrderId.HasValue || passenger.OrderId == targetOrder.Id)
                {
                    continue;
                }

                var sourceOrder = await GetOrderWithPassengersAsync(passenger.OrderId.Value);
                if (sourceOrder.OrderStatus != OrderStatus.Pending && sourceOrder.OrderStatus != OrderStatus.Dispatched)
                {
                    throw new UserFriendlyException(L["Order:TransferSourceInvalid"]);
                }

                var linkedPassenger = sourceOrder.PassengerInfos.FirstOrDefault(x => x.Id == passenger.Id);
                if (linkedPassenger != null)
                {
                    sourceOrder.PassengerInfos.Remove(linkedPassenger);
                }

                passenger.OrderId = targetOrder.Id;
                passenger.Status = PassengerStatus.Dispatched;

                if (targetOrder.PassengerInfos.All(x => x.Id != passenger.Id))
                {
                    targetOrder.PassengerInfos.Add(passenger);
                }

                await _orderRepository.UpdateAsync(sourceOrder, autoSave: true);
                await _dispatchLogRepository.InsertAsync(new DispatchLog
                {
                    PassengerId = passenger.Id,
                    PassengerName = passenger.Name,
                    SourceOrderId = sourceOrder.Id,
                    TargetOrderId = targetOrder.Id,
                    Reason = input.Reason,
                    OperatorId = CurrentUser.Id,
                    OperatorName = CurrentUser.UserName
                }, autoSave: true);
            }

            await _orderRepository.UpdateAsync(targetOrder, autoSave: true);
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
            else if (order.OrderStatus == OrderStatus.Dispatched)
            {
                order.OrderStatus = OrderStatus.IsOnTheWay;
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
            var order = await GetOrderWithPassengersAsync(orderId);
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
                    .ThenInclude(x => x.Hotel)
                .Include(x => x.Driver);

            if (input.OrderType.HasValue)
            {
                query = query.Where(x => x.OrderType == input.OrderType.Value);
            }

            if (input.OrderId.HasValue)
            {
                query = query.Where(x => x.Id == input.OrderId.Value);
            }

            if (input.DriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == input.DriverId.Value);
            }

            if (input.OrderStatus != null && input.OrderStatus.Count > 0)
            {
                query = query.Where(x => input.OrderStatus.Contains(x.OrderStatus));
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

        private async Task<Order> GetOrderWithPassengersAsync(long orderId)
        {
            var queryable = await _orderRepository.WithDetailsAsync();
            var order = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Include(x => x.PassengerInfos).Include(x => x.Vehicle).Include(x => x.Driver),
                x => x.Id == orderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            return order;
        }
    }
}
