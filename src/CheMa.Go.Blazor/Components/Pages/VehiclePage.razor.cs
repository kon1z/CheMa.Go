using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;

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
    }
}
