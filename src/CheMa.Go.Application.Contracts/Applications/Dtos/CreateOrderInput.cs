using CheMa.Go.Domain.Enums;
using System;
using Volo.Abp.Identity;

namespace CheMa.Go.Applications.Dtos;

public class CreateOrderInput
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
    public PassengerDto Passenger { get; set; } = null!;
    /// <summary>
    /// 航班信息
    /// </summary>
    public FlightInfoDto FlightInfo { get; set; }
    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime AppointmentTime { get; set; }

    /// <summary>
    /// 接送机车辆
    /// </summary>
    public VehicleDto Vehicle { get; set; }
    /// <summary>
    /// 接送机司机
    /// </summary>
    public IdentityUserDto Driver { get; set; } = null!;
    /// <summary>
    /// 酒店信息
    /// </summary>
    public HotelDto Hotel { get; set; } = null!;
}