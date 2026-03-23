using CheMa.Go.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace CheMa.Go.Permissions;

public class GoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GoPermissions.GroupName);

        var orderPermission = myGroup.AddPermission(GoPermissions.Orders.Default, L("Permission:Orders"));
        orderPermission.AddChild(GoPermissions.Orders.ConfirmDispatch, L("Permission:Orders.ConfirmDispatch"));
        orderPermission.AddChild(GoPermissions.Orders.ForceTransfer, L("Permission:Orders.ForceTransfer"));

        myGroup.AddPermission(GoPermissions.DispatchLogs.Default, L("Permission:DispatchLogs"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GoResource>(name);
    }
}
