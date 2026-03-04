using Blazorise;
using System.Collections.Generic;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Applications.AppServices;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

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

        Modal DispatchVehicleModal { get; set; } = null!;
        List<VehicleDto> DispatchVehicles { get; set; } = new();
        OrderDto? DispatchOrder { get; set; }
        long SelectedVehicleId { get; set; }

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
    }
}
