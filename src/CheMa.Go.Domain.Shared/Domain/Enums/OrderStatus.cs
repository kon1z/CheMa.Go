namespace CheMa.Go.Domain.Enums;

public enum OrderStatus
{
    /// <summary>
    /// 待确认
    /// </summary>
    Pending,
    /// <summary>
    /// 已派单
    /// </summary>
    Dispatched,
    /// <summary>
    /// 司机在路上
    /// </summary>
    IsOnTheWay,
    /// <summary>
    /// 司机已到达
    /// </summary>
    Arrived,
    /// <summary>
    /// 乘客已上车
    /// </summary>
    Picked,
    /// <summary>
    /// 已完成
    /// </summary>
    Completed,
    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled,
    /// <summary>
    /// 已拒绝
    /// </summary>
    Rejected,
}