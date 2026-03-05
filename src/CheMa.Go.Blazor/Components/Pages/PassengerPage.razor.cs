using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class PassengerPage
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        public PassengerPage()
        {
            LocalizationResource = typeof(GoResource);
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchPassengersAsync()
        {
            await SearchEntitiesAsync();
        }

        private async Task ClearPassengerSearchAsync()
        {
            GetListInput.Name = null;
            GetListInput.Phone = null;
            GetListInput.Status = null;
            GetListInput.OrderId = null;
            await SearchEntitiesAsync();
        }
    }
}
