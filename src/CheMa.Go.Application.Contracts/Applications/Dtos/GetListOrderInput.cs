using CheMa.Go.Domain.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class GetListOrderInput : PagedAndSortedResultRequestDto
{
    public long? OrderId { get; set; }
    public Guid? DriverId { get; set; }
    public OrderType? OrderType { get; set; }
    public List<OrderStatus>? OrderStatus { get; set; }
    public DateTime? AppointmentStartTime { get; set; }
    public DateTime? AppointmentEndTime { get; set; }
    public string? LicenseNum { get; set; }
}