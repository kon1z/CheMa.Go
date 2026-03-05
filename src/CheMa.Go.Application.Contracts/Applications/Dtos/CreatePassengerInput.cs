using System;
using CheMa.Go.Domain.Enums;

namespace CheMa.Go.Applications.Dtos;

public class CreatePassengerInput
{
    /// <summary>
    /// 乘客接送状态
    /// </summary>
    public PassengerStatus Status { get; set; } = PassengerStatus.PendingPickup;

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
    public DateTime AppointmentTime { get; set; } = DateTime.Today;

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}