using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;

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
    }
}
