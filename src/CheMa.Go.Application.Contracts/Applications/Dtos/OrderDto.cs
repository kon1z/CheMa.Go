using CheMa.Go.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 已排乘客数量
    /// </summary>
    public string DispatchedPassengerCount => PassengerInfos.Sum(x => x.Count) + (Vehicle == null ? "" : $"/{Vehicle.SeatCount}");

    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime AppointmentTime { get; set; }

    public LocationInfoDto Location { get; set; } = new();

    /// <summary>
    /// 接送机车辆
    /// </summary>
    public VehicleDto? Vehicle { get; set; }

    /// <summary>
    /// 接送机司机
    /// </summary>
    public IdentityUserDto? Driver { get; set; }

    public string? DriverName => Driver?.UserName;
}
