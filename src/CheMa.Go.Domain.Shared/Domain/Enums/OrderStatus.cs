namespace CheMa.Go.Domain.Enums;

public enum OrderStatus
{
    Pending,
    Dispatched,
    IsOnTheWay,
    Arrived,
    PartiallyPicked,
    Picked,
    Completed,
    Cancelled,
    Rejected,
}