using Volo.Abp.Application.Dtos;
using CheMa.Go.Domain.Enums;

namespace CheMa.Go.Applications.Dtos;

public class GetListPassengerInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public PassengerStatus? Status { get; set; }
    public long? OrderId { get; set; }
}
