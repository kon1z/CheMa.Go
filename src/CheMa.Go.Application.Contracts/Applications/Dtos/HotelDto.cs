using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace CheMa.Go.Applications.Dtos;

public class HotelDto : EntityDto<long>
{
    /// <summary>
    /// 酒店
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 所属用户
    /// </summary>
    public List<IdentityUserDto> HotelUsers { get; set; } = new();
}