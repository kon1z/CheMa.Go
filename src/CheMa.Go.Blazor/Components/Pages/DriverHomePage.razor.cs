using Blazorise;
using CheMa.Go.Applications.AppServices;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Enums;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class DriverHomePage
    {
        [Inject]
        protected IOrderAppService OrderAppService { get; set; } = null!;

        [Inject]
        protected IVehicleAppServices VehicleAppService { get; set; } = null!;

        [Inject]
        protected IPassengerAppService PassengerAppService { get; set; } = null!;

        List<OrderDto> Orders { get; set; } = new();
        Modal DispatchVehicleModal { get; set; } = null!;
        List<VehicleDto> DispatchVehicles { get; set; } = new();
        OrderDto? DispatchOrder { get; set; }
        long SelectedVehicleId { get; set; }

        public DriverHomePage()
        {
            LocalizationResource = typeof(GoResource);
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            if (!CurrentUser.Id.HasValue)
            {
                Orders = new List<OrderDto>();
                return;
            }

            var result = await OrderAppService.GetListAsync(new GetListOrderInput
            {
                DriverId = CurrentUser.Id.Value,
                MaxResultCount = 1000,
                Sorting = nameof(OrderDto.AppointmentTime) + " desc"
            });

            Orders = result.Items.Where(x => x.OrderStatus != OrderStatus.Pending).ToList();
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

            await OrderAppService.LinkVehicleToOrderAsync(new LinkVehicleToOrderInput
            {
                OrderId = DispatchOrder.Id,
                VehicleId = SelectedVehicleId == 0 ? null : SelectedVehicleId
            });

            await DispatchVehicleModal.Hide();
            await LoadOrdersAsync();
        }

        private bool DisplayDetailRow(OrderDto order)
        {
            return order.PassengerInfos.Count > 0;
        }

        private async Task SetPassengerBoardedAsync(PassengerDto passenger)
        {
            await PassengerAppService.SetBoardedAsync(passenger.Id);
            await LoadOrdersAsync();
        }

        private async Task SetPassengerExitAsync(PassengerDto passenger)
        {
            await PassengerAppService.SetPassengerExitAsync(passenger.Id);
            await LoadOrdersAsync();
        }
    }
}
