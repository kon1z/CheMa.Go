using System;

namespace CheMa.Go.Applications.Dtos;

public class LinkDriverToOrderInput
{
    public long OrderId { get; set; }

    public Guid? DriverId { get; set; }
}
