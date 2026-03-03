using System.Threading.Tasks;
using CheMa.Go.Applications.Dtos;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IVehicleAppServices : ICrudAppService<VehicleDto, long, GetListVehicleInput,
    CreateVehicleInput, UpdateVehicleInput>
{
    Task<bool> IsLicenseNumExistAsync(string licenseNum);
}