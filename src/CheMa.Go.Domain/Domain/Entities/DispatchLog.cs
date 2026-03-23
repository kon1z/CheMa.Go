using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CheMa.Go.Domain.Entities;

public class DispatchLog : FullAuditedAggregateRoot<long>
{
    public long PassengerId { get; set; }

    public string PassengerName { get; set; } = string.Empty;

    public long SourceOrderId { get; set; }

    public long TargetOrderId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public Guid? OperatorId { get; set; }

    public string? OperatorName { get; set; }
}
