using System;
using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos;

public class DispatchLogDto : EntityDto<long>
{
    public long PassengerId { get; set; }

    public string PassengerName { get; set; } = string.Empty;

    public long SourceOrderId { get; set; }

    public long TargetOrderId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string? OperatorName { get; set; }

    public DateTime CreationTime { get; set; }
}
