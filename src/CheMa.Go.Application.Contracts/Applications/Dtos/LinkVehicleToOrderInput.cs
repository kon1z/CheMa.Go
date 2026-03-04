namespace CheMa.Go.Applications.Dtos;

public class LinkVehicleToOrderInput
{
    public long OrderId { get; set; }

    public long? VehicleId { get; set; }
}
