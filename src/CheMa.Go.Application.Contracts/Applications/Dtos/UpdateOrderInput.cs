using CheMa.Go.Domain.Enums;
using System;

namespace CheMa.Go.Applications.Dtos;

public class UpdateOrderInput
{
    /// <summary>
    /// 订单类型（接机、送机）
    /// </summary>
    public OrderType OrderType { get; set; }
    /// <summary>
    /// 订单来源
    /// </summary>
    public OrderSource OrderSource { get; set; }
    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime AppointmentTime { get; set; }

    public LocationInfoDto Location { get; set; } = new();
}