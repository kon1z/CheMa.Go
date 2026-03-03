using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Localization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices
{
    public class VehicleAppServices :
        CrudAppService<Vehicle, VehicleDto, long, GetListVehicleInput, CreateVehicleInput,
            UpdateVehicleInput>, IVehicleAppServices
    {
        public VehicleAppServices(IRepository<Vehicle, long> repository) : base(repository)
        {
            LocalizationResource = typeof(GoResource);
        }

        public Task<bool> IsLicenseNumExistAsync(string licenseNum)
        {
            return Repository.AnyAsync(x => x.LicenseNum == licenseNum);
        }

        public override async Task<VehicleDto> CreateAsync(CreateVehicleInput input)
        {
            var isExist = await IsLicenseNumExistAsync(input.LicenseNum);
            if (isExist)
            {
                throw new UserFriendlyException("车牌号重复");
            }

            return await base.CreateAsync(input);
        }
    }
}
