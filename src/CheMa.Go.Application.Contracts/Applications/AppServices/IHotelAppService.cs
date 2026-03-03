using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheMa.Go.Applications.Dtos;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IHotelAppService : ICrudAppService<HotelDto, long, GetListHotelInput, CreateHotelInput, UpdateHotelInput>
{
    Task<List<HotelDto>> GetCurrentUserBelongHotelAsync();
    Task<HotelDto> GetListHotelUsersAsync(long hotelId);
    Task LinkUsersToHotelAsync(LinkUsersToHotelInput input);
    Task RemoteUserFromHotelAsync(long hotelId, Guid userId);
}