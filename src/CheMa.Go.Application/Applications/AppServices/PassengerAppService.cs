using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
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

        protected override async Task<IQueryable<Passenger>> CreateFilteredQueryAsync(GetListPassengerInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                query = query.Where(x => x.Name.Contains(input.Name));
            }

            if (!string.IsNullOrWhiteSpace(input.Phone))
            {
                query = query.Where(x => x.Phone.Contains(input.Phone));
            }

            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status.Value);
            }

            if (input.OrderId.HasValue)
            {
                query = query.Where(x => x.OrderId == input.OrderId.Value);
            }

            return query;
        }
    }
}
