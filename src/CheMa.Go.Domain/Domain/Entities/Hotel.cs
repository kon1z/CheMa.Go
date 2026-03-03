using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace CheMa.Go.Domain.Entities
{
    public class Hotel : AggregateRoot<long>
    {
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 酒店员工
        /// </summary>
        public virtual IList<IdentityUser> HotelUsers { get; set; } = new List<IdentityUser>();
    }
}
