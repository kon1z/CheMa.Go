using Blazorise;
using Blazorise.Components;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheMa.Go.Applications.AppServices;
using Volo.Abp.Identity;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class OrderPage
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        Modal LinkPassengerModal { get; set; }

        private long currentOrderId;
        private OrderDto? currentOrder;

        LinkPassengersToOrderInput LinkPassengersToOrderInput { get; set; } = new();
        string LinkedOrderName { get; set; } = string.Empty;

        IEnumerable<PassengerDto> FilteredPassengers { get; set; } = new List<PassengerDto>();
        int TotalPassengersCount { get; set; }
        IEnumerable<IdentityUserDto> FilteredDrivers { get; set; } = new List<IdentityUserDto>();
        int TotalDriversCount { get; set; }
        Guid? SelectedDriverIdForCreate { get; set; }

        public OrderPage()
        {
            LocalizationResource = typeof(GoResource);
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchOrdersAsync()
        {
            await SearchEntitiesAsync();
        }

        private async Task OnDriversReadData(AutocompleteReadDataEventArgs e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            var userAppService = ScopedServices.GetRequiredService<IIdentityUserAppService>();
            var result = await userAppService.GetListAsync(new GetIdentityUsersInput
            {
                MaxResultCount = e.VirtualizeCount,
                SkipCount = e.VirtualizeOffset,
                Filter = e.SearchValue
            });

            FilteredDrivers = result.Items;
            TotalDriversCount = (int)result.TotalCount;
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

        public async Task OpenLinkPassengerModalAsync(OrderDto entity)
        {
            currentOrder = entity;
            currentOrderId = entity.Id;
            LinkPassengersToOrderInput.OrderId = entity.Id;
            LinkPassengersToOrderInput.PassengerIds ??= new List<long>();

            var orderDto = await AppService.GetListOrderPassengersAsync(entity.Id);
            LinkedOrderName = $"#{orderDto.Id}";
            LinkPassengersToOrderInput.PassengerIds = orderDto.PassengerInfos.Select(x => x.Id).ToList();

            await InvokeAsync(LinkPassengerModal.Show);
        }

        private Task CloseLinkPassengerModalAsync(MouseEventArgs arg)
        {
            return LinkPassengerModal.Hide();
        }

        private async Task OnPassengersReadData(AutocompleteReadDataEventArgs e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            var passengerAppService = ScopedServices.GetRequiredService<IPassengerAppService>();
            var result = await passengerAppService.GetListAsync(new GetListPassengerInput
            {
                MaxResultCount = e.VirtualizeCount,
                SkipCount = e.VirtualizeOffset,
                Name = e.SearchValue,
                Phone = e.SearchValue
            });

            FilteredPassengers = result.Items;
            TotalPassengersCount = (int)result.TotalCount;
        }

        public async Task LinkPassengersToOrderAsync()
        {
            LinkPassengersToOrderInput.OrderId = currentOrderId;
            await AppService.LinkPassengersToOrderAsync(LinkPassengersToOrderInput);

            var orderDto = await AppService.GetListOrderPassengersAsync(currentOrderId);
            if (currentOrder != null)
            {
                currentOrder.PassengerInfos = orderDto.PassengerInfos;
            }

            await LinkPassengerModal.Hide();
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
