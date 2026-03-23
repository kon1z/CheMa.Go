namespace CheMa.Go.Permissions;

public static class GoPermissions
{
    public const string GroupName = "Go";

    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string ConfirmDispatch = Default + ".ConfirmDispatch";
        public const string ForceTransfer = Default + ".ForceTransfer";
    }

    public static class DispatchLogs
    {
        public const string Default = GroupName + ".DispatchLogs";
    }
}
