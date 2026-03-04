using Blazorise;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
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

        Modal LinkPassengerModal { get; set; }
     

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
    }
}
