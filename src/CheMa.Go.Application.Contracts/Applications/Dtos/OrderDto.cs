using CheMa.Go.Domain.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace CheMa.Go.Applications.Dtos;

public class OrderDto : EntityDto<long>
{
    /// <summary>
    /// 订单类型（接机、送机）
    /// </summary>
    public OrderType OrderType { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// 订单来源
    /// </summary>
    public OrderSource OrderSource { get; set; }

    /// <summary>
    /// 乘客信息
    /// </summary>
    public List<PassengerDto> PassengerInfos { get; set; } = new();

    /// <summary>
    /// 航班信息
    /// </summary>
    public FlightInfoDto? FlightInfo { get; set; }

    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime AppointmentTime { get; set; }

    /// <summary>
    /// 接送机车辆
    /// </summary>
    public VehicleDto Vehicle { get; set; } = null!;

    /// <summary>
    /// 接送机司机
    /// </summary>
    public IdentityUserDto Driver { get; set; } = null!;

    /// <summary>
    /// 送车司机称呼
    /// </summary>
    public string DriverName => string.IsNullOrWhiteSpace(Driver?.Name) ? Driver?.UserName ?? string.Empty : Driver.Name!;
}
