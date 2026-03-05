namespace CheMa.Go.Domain.Enums;

public enum PassengerStatus
{
    /// <summary>
    /// 未接送
    /// </summary>
    PendingPickup,
    /// <summary>
    /// 已派单
    /// </summary>
    Dispatched,
    /// <summary>
    /// 已上车
    /// </summary>
    Boarded,
    /// <summary>
    /// 接送完成
    /// </summary>
    Completed,
}
