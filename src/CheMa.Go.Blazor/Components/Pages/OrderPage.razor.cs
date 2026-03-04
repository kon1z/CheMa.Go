using Blazorise;
using System.Collections.Generic;
using System;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Applications.AppServices;
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

        Modal DispatchVehicleModal { get; set; } = null!;
        Modal DispatchDriverModal { get; set; } = null!;
        List<VehicleDto> DispatchVehicles { get; set; } = new();
        List<IdentityUserDto> DispatchDrivers { get; set; } = new();
        OrderDto? DispatchOrder { get; set; }
        long SelectedVehicleId { get; set; }
        Guid SelectedDriverId { get; set; }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchOrdersAsync()
        {
            await SearchEntitiesAsync();
        }

        private async Task ClearOrderSearchAsync()
        {
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

        private async Task RemovePassengerFromOrderAsync(PassengerDto passenger, OrderDto order)
        {
            await AppService.RemovePassengerFromOrderAsync(order.Id, passenger.Id);
            order.PassengerInfos.RemoveAll(x => x.Id == passenger.Id);
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
    }
}
