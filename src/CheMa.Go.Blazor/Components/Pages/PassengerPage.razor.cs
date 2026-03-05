using CheMa.Go.Applications.AppServices;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class PassengerPage
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        [Inject]
        protected IHotelAppService HotelAppService { get; set; } = null!;

        List<HotelDto> CurrentUserHotels { get; set; } = new();

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

        private async Task OpenCreatePassengerModalAsync()
        {
            CurrentUserHotels = await HotelAppService.GetCurrentUserBelongHotelAsync();

            if (CurrentUserHotels.Count == 0)
            {
                await Message.Error(L["Passenger:NoOwnedHotel"]);
                return;
            }

            await OpenCreateModalAsync();


            NewEntity.HotelId = CurrentUserHotels.Count > 0 ? CurrentUserHotels.First().Id : 0;
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
