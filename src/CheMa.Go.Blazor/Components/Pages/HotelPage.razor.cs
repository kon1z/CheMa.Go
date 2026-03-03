using Blazorise;
using Blazorise.Components;
using CheMa.Go.Applications.Dtos;
using CheMa.Go.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace CheMa.Go.Blazor.Components.Pages
{
    public partial class HotelPage
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        // 用于链接用户的模态框引用
        Modal LinkUserModal { get; set; }

        // 当前操作的酒店 ID
        private long currentHotelId;
        private HotelDto? currentHotel;

        // 链接用户输入 DTO
        LinkUsersToHotelInput LinkUsersToHotelInput { get; set; } = new LinkUsersToHotelInput();

        // 显示的酒店名称
        string LinkedHotelName { get; set; }

        // 用户列表相关
        IEnumerable<IdentityUserDto> FilteredUsers { get; set; } = new List<IdentityUserDto>();
        int TotalUsersCount { get; set; }

        public HotelPage()
        {
            LocalizationResource = typeof(GoResource);
        }

        protected override void Dispose(bool disposing)
        {
            PageLayout.ShowToolbar = true;
        }

        private async Task SearchHotelsAsync()
        {
            await SearchEntitiesAsync();
        }

        private async Task ClearHotelSearchAsync()
        {
            GetListInput.Filter = null;
            await SearchEntitiesAsync();
        }

        /// <summary>
        /// 打开链接用户模态框
        /// </summary>
        public async Task OpenLinkUserModalAsync(HotelDto entity)
        {
            currentHotel = entity;
            currentHotelId = entity.Id;
            LinkUsersToHotelInput.HotelId = entity.Id;
            LinkUsersToHotelInput.UserIds ??= new List<Guid>();

            // 获取酒店详情（包括名称）
            var hotelDto = await AppService.GetListHotelUsersAsync(entity.Id); // 假设此方法返回 HotelDto
            LinkedHotelName = hotelDto.Name;

            // 清空之前的选择
            LinkUsersToHotelInput.UserIds.Clear();

            // 可选：预加载当前酒店已链接的用户 ID
            // var linkedUserIds = await AppService.GetLinkedUserIdsAsync(entity.Id);
            // LinkUsersToHotelInput.UserIds = linkedUserIds.ToList();

            await InvokeAsync(LinkUserModal.Show);
        }

        /// <summary>
        /// 关闭链接用户模态框
        /// </summary>
        private Task CloseLinkUserModalAsync(MouseEventArgs arg)
        {
            return LinkUserModal.Hide();
        }

        /// <summary>
        /// 处理用户下拉列表的远程搜索（分页 + 过滤）
        /// </summary>
        private async Task OnUsersReadData(AutocompleteReadDataEventArgs e)
        {
            if (e.CancellationToken.IsCancellationRequested)
                return;

            var userAppService = ScopedServices.GetRequiredService<IIdentityUserAppService>();

            var result = await userAppService.GetListAsync(new GetIdentityUsersInput
            {
                MaxResultCount = e.VirtualizeCount,
                SkipCount = e.VirtualizeOffset,
                Filter = e.SearchValue
            });

            FilteredUsers = result.Items;
            TotalUsersCount = (int)result.TotalCount;
        }

        /// <summary>
        /// 保存用户与酒店的关联
        /// </summary>
        public async Task LinkUsersToHotelAsync()
        {
            // 确保 HotelId 正确
            LinkUsersToHotelInput.HotelId = currentHotelId;

            await AppService.LinkUsersToHotelAsync(LinkUsersToHotelInput);
            var hotelDto = await AppService.GetListHotelUsersAsync(currentHotelId);
            if (currentHotel != null)
            {
                currentHotel.HotelUsers = hotelDto.HotelUsers;
            }

            await LinkUserModal.Hide();
        }

        /// <summary>
        /// 是否展示详细栏位
        /// </summary>
        /// <param name="argItem"></param>
        /// <returns></returns>
        private bool DisplayDetailRow(HotelDto argItem)
        {
            return argItem.HotelUsers.Count > 0;
        }

        /// <summary>
        /// 移除酒店员工关联信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hotel"></param>
        /// <returns></returns>
        private async Task RemoveUserFromHotelAsync(IdentityUserDto user, HotelDto hotel)
        {
            await AppService.RemoteUserFromHotelAsync(hotel.Id, user.Id);
            hotel.HotelUsers.RemoveAll(x => x.Id == user.Id);
        }
    }
}
