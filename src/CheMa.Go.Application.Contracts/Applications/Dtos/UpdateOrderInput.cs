using CheMa.Go.Domain.Enums;
using System;

namespace CheMa.Go.Applications.Dtos;

public class UpdateOrderInput
{
    public OrderType OrderType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public OrderSource OrderSource { get; set; }
    public DateTime AppointmentTime { get; set; }
}