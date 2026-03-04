using Volo.Abp.Application.Dtos;
using System;
using CheMa.Go.Domain.Enums;

namespace CheMa.Go.Applications.Dtos;

public class GetListOrderInput : PagedAndSortedResultRequestDto
{
    public OrderType? OrderType { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public DateTime? AppointmentStartTime { get; set; }
    public DateTime? AppointmentEndTime { get; set; }
    public string? LicenseNum { get; set; }
}