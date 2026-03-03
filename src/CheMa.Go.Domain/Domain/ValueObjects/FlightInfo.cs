using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Values;

namespace CheMa.Go.Domain.ValueObjects;

[ComplexType]
public class FlightInfo : ValueObject
{
    public string? FlightNumber { get; set; } = string.Empty;
    public string? DepartureAirport { get; set; } = string.Empty;
    public string? ArrivalAirport { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FlightNumber;
        yield return DepartureAirport;
        yield return ArrivalAirport;
        yield return DepartureTime;
        yield return ArrivalTime;
    }
}