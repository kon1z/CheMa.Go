using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheMa.Go.Applications.Dtos;

public class ForceTransferPassengerInput
{
    public long TargetOrderId { get; set; }

    public List<long> PassengerIds { get; set; } = new();

    [Required]
    [MaxLength(255)]
    public string Reason { get; set; } = string.Empty;
}
