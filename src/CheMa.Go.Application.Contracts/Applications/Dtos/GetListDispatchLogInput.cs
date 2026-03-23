using System;
using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class GetListDispatchLogInput : PagedAndSortedResultRequestDto
{
    public long? OrderId { get; set; }

    public long? PassengerId { get; set; }

    public string? OperatorName { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
