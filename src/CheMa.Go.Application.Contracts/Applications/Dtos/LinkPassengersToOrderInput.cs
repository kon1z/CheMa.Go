using System.Collections.Generic;

namespace CheMa.Go.Applications.Dtos;

public class LinkPassengersToOrderInput
{
    public long OrderId { get; set; }

    public List<long> PassengerIds { get; set; } = new();
}
