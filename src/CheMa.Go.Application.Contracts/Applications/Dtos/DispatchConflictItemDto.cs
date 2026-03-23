using System;
using CheMa.Go.Domain.Enums;

namespace CheMa.Go.Applications.Dtos;

public class DispatchConflictItemDto
{
    public long PassengerId { get; set; }

    public string PassengerName { get; set; } = string.Empty;

    public long OrderId { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public DateTime AppointmentTime { get; set; }

    public string? DriverName { get; set; }

    public string? VehicleLicenseNum { get; set; }
}
