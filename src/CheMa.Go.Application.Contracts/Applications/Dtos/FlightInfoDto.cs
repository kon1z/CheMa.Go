using System;

namespace CheMa.Go.Applications.Dtos
{
    public class FlightInfoDto
    {
        /// <summary>
        /// 航班号
        /// </summary>
        public string? FlightNumber { get; set; }
        /// <summary>
        /// 出发机场
        /// </summary>
        public string? DepartureAirport { get; set; }
        /// <summary>
        /// 到达机场
        /// </summary>
        public string? ArrivalAirport { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime? DepartureTime { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime? ArrivalTime { get; set; }
    }
}
