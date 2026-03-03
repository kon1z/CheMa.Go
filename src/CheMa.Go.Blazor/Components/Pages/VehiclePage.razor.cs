using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class VehiclePage
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        public VehiclePage()
        {
            LocalizationResource = typeof(GoResource);
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchVehiclesAsync()
        {
            await SearchEntitiesAsync();
        }

        private async Task ClearVehicleSearchAsync()
        {
            GetListInput.LicenseNum = null;
            GetListInput.SeatCount = null;
            GetListInput.Name = null;
            await SearchEntitiesAsync();
        }
    }
}
