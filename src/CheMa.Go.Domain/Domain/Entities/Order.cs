using CheMa.Go.Domain.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace CheMa.Go.Domain.Entities
{
    public class Order : FullAuditedAggregateRoot<long>
    {
        /// <summary>
        /// 订单类型（接机、送机）
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource OrderSource { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }

        /// <summary>
        /// 接送机Id
        /// </summary>
        public long? VehicleId { get; set; }

        /// <summary>
        /// 接送机车辆
        /// </summary>
        public virtual Vehicle? Vehicle { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public Guid? DriverId { get; set; }

        /// <summary>
        /// 司机
        /// </summary>
        public virtual IdentityUser? Driver { get; set; }

        /// <summary>
        /// 乘客信息
        /// </summary>
        public virtual IList<Passenger> PassengerInfos { get; set; } = new List<Passenger>();
    }
}
