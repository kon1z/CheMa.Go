using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace CheMa.Go.Applications.AppServices
{
    public class HotelAppService : CrudAppService<Hotel, HotelDto, long, GetListHotelInput, CreateHotelInput, UpdateHotelInput>, IHotelAppService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public HotelAppService(IRepository<Hotel, long> repository,
            IHotelRepository hotelRepository,
            IIdentityUserRepository identityUserRepository) 
            : base(repository)
        {
            _hotelRepository = hotelRepository;
            _identityUserRepository = identityUserRepository;
        }

        [Authorize]
        public async Task<List<HotelDto>> GetCurrentUserBelongHotelAsync()
        {
            var hotels = await _hotelRepository.GetCurrentUserBelongHotelAsync(CurrentUser.Id);

            return ObjectMapper.Map<List<Hotel>, List<HotelDto>>(hotels);
        }

        public async Task<HotelDto> GetListHotelUsersAsync(long hotelId)
        {
            var queryable = await _hotelRepository.WithDetailsAsync();
            var hotel = await AsyncExecuter.FirstOrDefaultAsync(queryable, x => x.Id == hotelId);
            if (hotel == null)
            {
                throw new EntityNotFoundException(typeof(Hotel), hotelId);
            }

            return ObjectMapper.Map<Hotel, HotelDto>(hotel);
        }

        public async Task LinkUsersToHotelAsync(LinkUsersToHotelInput input)
        {
            var queryable = await _hotelRepository.WithDetailsAsync();
            var hotel = await AsyncExecuter.FirstOrDefaultAsync(queryable, x => x.Id == input.HotelId);
            if (hotel == null)
            {
                throw new EntityNotFoundException(typeof(Hotel), input.HotelId);
            }

            var users = await _identityUserRepository.GetListByIdsAsync(input.UserIds);
            foreach (var user in users)
            {
                hotel.HotelUsers.AddIfNotContains(user);
            }

            await _hotelRepository.UpdateAsync(hotel, autoSave: true);
        }

        public async Task RemoteUserFromHotelAsync(long hotelId, Guid userId)
        {
            var identityUser = await _identityUserRepository.GetAsync(userId);
            await _hotelRepository.RemoteUserFromHotelAsync(hotelId, identityUser);
        }

        protected override async Task<IQueryable<Hotel>> CreateFilteredQueryAsync(GetListHotelInput input)
        {
            var filteredQueryAsync = await base.CreateFilteredQueryAsync(input);
            IQueryable<Hotel> query = filteredQueryAsync.Include(x => x.HotelUsers);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => x.Name.Contains(input.Filter!));
            }

            return query;
        }
    }
}
