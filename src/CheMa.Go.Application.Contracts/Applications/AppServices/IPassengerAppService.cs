using CheMa.Go.Applications.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IPassengerAppService : ICrudAppService<PassengerDto, long, GetListPassengerInput, CreatePassengerInput, UpdatePassengerInput>
{
    Task SetBoardedAsync(long id);
    Task SetPassengerExitAsync(long id);
    Task SetExceptionClosedAsync(long id);
}