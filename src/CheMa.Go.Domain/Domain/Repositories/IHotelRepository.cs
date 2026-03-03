using CheMa.Go.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace CheMa.Go.Domain.Repositories
{
    public interface IHotelRepository : IRepository<Hotel, long>
    {
        Task<List<Hotel>> GetCurrentUserBelongHotelAsync(Guid? currentUserId);
        Task RemoteUserFromHotelAsync(long hotelId, IdentityUser user);
    }
}
