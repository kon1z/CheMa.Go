using CheMa.Go.Applications.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CheMa.Go.Applications.AppServices;

public interface IDispatchLogAppService : IApplicationService
{
    Task<PagedResultDto<DispatchLogDto>> GetListAsync(GetListDispatchLogInput input);
}
