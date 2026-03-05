using System;
using CheMa.Go.Domain.Enums;
using Volo.Abp.Domain.Entities;

namespace CheMa.Go.Domain.Entities;

public class Passenger : AggregateRoot<long>
{
    /// <summary>
    /// 乘客接送状态
    /// </summary>
    public PassengerStatus Status { get; set; }

    /// <summary>
    /// 乘客称呼
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 乘客联系方式
    /// </summary>
    public string Phone { get; set; } = null!;
    /// <summary>
    /// 乘客数量
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime AppointmentTime { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 所属订单Id
    /// </summary>
    public long? OrderId { get; set; }

    /// <summary>
    /// 所属酒店
    /// </summary>
    public long HotelId { get; set; }

    /// <summary>
    /// 所属酒店
    /// </summary>
    public virtual Hotel? Hotel { get; set; }

    /// <summary>
    /// 所属订单
    /// </summary>
    public virtual Order? Order { get; set; }
}