using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos
{
    public class VehicleDto : EntityDto<long>
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
}
