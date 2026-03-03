using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices
{
    public class PassengerAppService :
        CrudAppService<Passenger, PassengerDto, long, GetListPassengerInput, CreatePassengerInput,
            UpdatePassengerInput>, IPassengerAppService
    {
        public PassengerAppService(IRepository<Passenger, long> repository) : base(repository)
        {
        }
    }
}
 