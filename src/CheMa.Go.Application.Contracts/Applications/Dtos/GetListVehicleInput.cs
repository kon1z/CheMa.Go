using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class GetListVehicleInput : PagedAndSortedResultRequestDto
{
    public string? LicenseNum { get; set; }
    public int? SeatCount { get; set; }
    public string? Name { get; set; }
}
