namespace CheMa.Go.Applications.Dtos;

public class UpdateVehicleInput
{
    /// <summary>
    /// 车型名称
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 座位数
    /// </summary>
    public int SeatCount { get; set; }
    /// <summary>
    /// 车牌号
    /// </summary>
    public string LicenseNum { get; set; } = null!;
}