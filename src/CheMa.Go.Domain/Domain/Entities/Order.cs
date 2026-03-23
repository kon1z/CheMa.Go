using CheMa.Go.Domain.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace CheMa.Go.Domain.Entities
{
    public class LocationInfo
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Detail { get; set; }
    }

    public class Order : FullAuditedAggregateRoot<long>
    {
        public OrderType OrderType { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public OrderSource OrderSource { get; set; }

        public DateTime AppointmentTime { get; set; }

        public LocationInfo Location { get; set; } = new();

        public long? VehicleId { get; set; }

        public virtual Vehicle? Vehicle { get; set; }

        public Guid? DriverId { get; set; }

        public virtual IdentityUser? Driver { get; set; }

        public virtual IList<Passenger> PassengerInfos { get; set; } = new List<Passenger>();
    }
}
