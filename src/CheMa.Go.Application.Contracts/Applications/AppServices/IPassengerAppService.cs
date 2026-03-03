using CheMa.Go.Applications.Dtos;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IPassengerAppService : ICrudAppService<PassengerDto, long, GetListPassengerInput, CreatePassengerInput, UpdatePassengerInput>
{
}