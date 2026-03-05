using CheMa.Go.Localization;
using CheMa.Go.MultiTenancy;
using System.Threading.Tasks;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace CheMa.Go.Blazor.Menus;

public class GoMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<GoResource>();
        
        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                GoMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 1
            )
        );

        context.Menu.Items.Insert(
            1,
            new ApplicationMenuItem(
                GoMenus.DriverHome,
                l["Menu:DriverHome"],
                "/driver-home",
                icon: "fas fa-home",
                order: 1
            )
        );

        context.Menu.Items.Insert(
            2,
            new ApplicationMenuItem(
                GoMenus.Passengers,
                l["Menu:PassengerManagement"],
                "/passengers",
                icon: "fas fa-home",
                order: 1
            )
        );

        context.Menu.Items.Insert(
            3,
            new ApplicationMenuItem(
                GoMenus.Orders,
                l["Menu:OrderManagement"],
                "/orders",
                icon: "fas fa-home",
                order: 1
            )
        );

        context.Menu.Items.Insert(
            4,
            new ApplicationMenuItem(
                GoMenus.Vehicles,
                l["Menu:VehicleManagement"],
                "/vehicles",
                icon: "fas fa-home",
                order: 1
            )
        );

        context.Menu.Items.Insert(
            5,
            new ApplicationMenuItem(
                GoMenus.Hotels,
                l["Menu:HotelManagement"],
                "/hotels",
                icon: "fas fa-home",
                order: 1
            )
        );



        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
    
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}
