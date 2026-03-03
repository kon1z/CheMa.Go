using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class GetListHotelInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
