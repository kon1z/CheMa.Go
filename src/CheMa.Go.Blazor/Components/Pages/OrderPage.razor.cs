using Blazorise;
using System.Collections.Generic;
using System;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Applications.AppServices;
using CheMa.Go.Domain.Enums;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class OrderPage
    {
        public OrderPage()
        {
            LocalizationResource = typeof(GoResource);
        }

        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        [Inject]
        protected IVehicleAppServices VehicleAppService { get; set; } = null!;

        [Inject]
        protected IIdentityUserAppService IdentityUserAppService { get; set; } = null!;

        [Inject]
        protected IPassengerAppService PassengerAppService { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery(Name = "orderId")]
        public long? QueryOrderId { get; set; }

        Modal AddPassengerModal { get; set; } = null!;
        Modal DispatchVehicleModal { get; set; } = null!;
        Modal DispatchDriverModal { get; set; } = null!;
        List<PassengerDto> PassengersForOrder { get; set; } = new();
        List<VehicleDto> DispatchVehicles { get; set; } = new();
        List<IdentityUserDto> DispatchDrivers { get; set; } = new();
        OrderDto? DispatchOrder { get; set; }
        long SelectedVehicleId { get; set; }
        Guid SelectedDriverId { get; set; }
        List<long> SelectedPassengerIds { get; set; } = new();
        long? LastQueryOrderId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (QueryOrderId == LastQueryOrderId)
            {
                return;
            }

            LastQueryOrderId = QueryOrderId;
            GetListInput.OrderId = QueryOrderId;
            await SearchEntitiesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchOrdersAsync()
        {
            await SearchEntitiesAsync();
        }

        private bool IsOrderStatusSelected(OrderStatus status)
        {
            return GetListInput.OrderStatus?.Contains(status) == true;
        }

        private void OnOrderStatusSelectionChanged(ChangeEventArgs args)
        {
            var selectedValues = args.Value switch
            {
                string[] values => values,
                string value => new[] { value },
                _ => Array.Empty<string>()
            };

            var selectedStatuses = selectedValues
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .Select(x => (OrderStatus)x)
                .Distinct()
                .ToList();

            GetListInput.OrderStatus = selectedStatuses.Count == 0
                ? null
                : selectedStatuses;
        }

        private async Task ClearOrderSearchAsync()
        {
            GetListInput.OrderId = null;
            GetListInput.OrderType = null;
            GetListInput.OrderStatus = null;
            GetListInput.AppointmentStartTime = null;
            GetListInput.AppointmentEndTime = null;
            GetListInput.LicenseNum = null;
            await SearchEntitiesAsync();
        }

        private bool DisplayDetailRow(OrderDto argItem)
        {
            return argItem.PassengerInfos.Count > 0;
        }

        private static bool IsPendingOrder(OrderDto order)
        {
            return order.OrderStatus == OrderStatus.Pending;
        }

        private static bool IsDispatchEditable(OrderDto order)
        {
            return order.OrderStatus == OrderStatus.Pending || order.OrderStatus == OrderStatus.Dispatched;
        }

        private async Task RemovePassengerFromOrderAsync(PassengerDto passenger, OrderDto order)
        {
            await AppService.RemovePassengerFromOrderAsync(order.Id, passenger.Id);
            order.PassengerInfos.RemoveAll(x => x.Id == passenger.Id);
        }

        private async Task OpenAddPassengerModalAsync(OrderDto order)
        {
            DispatchOrder = order;
            SelectedPassengerIds = order.PassengerInfos.Select(x => x.Id).ToList();
            var passengerResult = await PassengerAppService.GetListAsync(new GetListPassengerInput
            {
                Status = PassengerStatus.PendingPickup,
                MaxResultCount = 1000
            });
            PassengersForOrder = passengerResult.Items.ToList();
            await AddPassengerModal.Show();
        }

        private async Task CloseAddPassengerModalAsync()
        {
            await AddPassengerModal.Hide();
        }

        private async Task AddPassengersToOrderAsync()
        {
            if (DispatchOrder == null)
            {
                return;
            }

            await AppService.LinkPassengersToOrderAsync(new LinkPassengersToOrderInput
            {
                OrderId = DispatchOrder.Id,
                PassengerIds = SelectedPassengerIds
            });

            DispatchOrder.PassengerInfos = PassengersForOrder
                .Where(x => SelectedPassengerIds.Contains(x.Id))
                .ToList();

            await AddPassengerModal.Hide();
            await SearchEntitiesAsync();
        }

        private void TogglePassengerSelection(long passengerId, bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedPassengerIds.Contains(passengerId))
                {
                    SelectedPassengerIds.Add(passengerId);
                }
                return;
            }

            SelectedPassengerIds.Remove(passengerId);
        }

        private void OnPassengerCheckedChanged(long passengerId, ChangeEventArgs args)
        {
            var isChecked = args.Value is bool boolValue
                ? boolValue
                : bool.TryParse(args.Value?.ToString(), out var parsed) && parsed;
            TogglePassengerSelection(passengerId, isChecked);
        }

        private async Task OpenDispatchVehicleModalAsync(OrderDto order)
        {
            DispatchOrder = order;
            SelectedVehicleId = order.Vehicle?.Id ?? 0;
            var vehicleResult = await VehicleAppService.GetListAsync(new GetListVehicleInput
            {
                MaxResultCount = 1000
            });
            DispatchVehicles = vehicleResult.Items.ToList();
            await DispatchVehicleModal.Show();
        }

        private async Task CloseDispatchVehicleModalAsync()
        {
            await DispatchVehicleModal.Hide();
        }

        private async Task DispatchVehicleToOrderAsync()
        {
            if (DispatchOrder == null)
            {
                return;
            }

            await AppService.LinkVehicleToOrderAsync(new LinkVehicleToOrderInput
            {
                OrderId = DispatchOrder.Id,
                VehicleId = SelectedVehicleId == 0 ? null : SelectedVehicleId
            });

            await DispatchVehicleModal.Hide();
            await SearchEntitiesAsync();
        }

        private async Task OpenDispatchDriverModalAsync(OrderDto order)
        {
            DispatchOrder = order;
            SelectedDriverId = order.Driver?.Id ?? Guid.Empty;
            var driverResult = await IdentityUserAppService.GetListAsync(new GetIdentityUsersInput
            {
                MaxResultCount = 1000
            });
            DispatchDrivers = driverResult.Items.ToList();
            await DispatchDriverModal.Show();
        }

        private async Task CloseDispatchDriverModalAsync()
        {
            await DispatchDriverModal.Hide();
        }

        private async Task DispatchDriverToOrderAsync()
        {
            if (DispatchOrder == null)
            {
                return;
            }

            await AppService.LinkDriverToOrderAsync(new LinkDriverToOrderInput
            {
                OrderId = DispatchOrder.Id,
                DriverId = SelectedDriverId == Guid.Empty ? null : SelectedDriverId
            });

            await DispatchDriverModal.Hide();
            await SearchEntitiesAsync();
        }

        private async Task ConfirmDispatchAsync(OrderDto order)
        {
            await AppService.ConfirmDispatchAsync(order.Id);
            order.OrderStatus = OrderStatus.Dispatched;
            await SearchEntitiesAsync();
        }
    }
}
