using System;
using System.Collections.Generic;

namespace CheMa.Go.Applications.Dtos;

public class LinkUsersToHotelInput
{
    public long HotelId { get; set; }
    public List<Guid> UserIds { get; set; } = new();
}