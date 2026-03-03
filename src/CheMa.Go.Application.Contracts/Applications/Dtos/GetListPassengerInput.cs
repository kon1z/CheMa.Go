using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class GetListPassengerInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
}
