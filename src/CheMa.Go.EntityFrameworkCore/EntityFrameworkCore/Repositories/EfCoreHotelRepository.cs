using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace CheMa.Go.EntityFrameworkCore.Repositories
{
    public class EfCoreHotelRepository : EfCoreRepository<GoDbContext, Hotel, long>, IHotelRepository
    {
        public EfCoreHotelRepository(IDbContextProvider<GoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Hotel>> GetCurrentUserBelongHotelAsync(Guid? currentUserId)
        {
            if (!currentUserId.HasValue)
            {
                return new List<Hotel>();
            }

            var queryable = await GetQueryableAsync();
            var hotels = await queryable.Include(x => x.HotelUsers)
                .Where(x => x.HotelUsers.Any(y => y.Id == currentUserId)).ToListAsync();

            return hotels;
        }

        public async Task RemoteUserFromHotelAsync(long hotelId, IdentityUser user)
        {
            var hotel = await GetAsync(hotelId);

            hotel.HotelUsers.Remove(user);
        }

        public override async Task<IQueryable<Hotel>> WithDetailsAsync()
        {
            var queryable = await base.WithDetailsAsync();
            return queryable.Include(x => x.HotelUsers);
        }
    }
}
