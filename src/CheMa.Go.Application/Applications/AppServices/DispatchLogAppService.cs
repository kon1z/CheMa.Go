using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using CheMa.Go.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CheMa.Go.Applications.AppServices;

[Authorize(GoPermissions.DispatchLogs.Default)]
public class DispatchLogAppService : ApplicationService, IDispatchLogAppService
{
    private readonly IRepository<DispatchLog, long> _dispatchLogRepository;

    public DispatchLogAppService(IRepository<DispatchLog, long> dispatchLogRepository)
    {
        _dispatchLogRepository = dispatchLogRepository;
    }

    public async Task<PagedResultDto<DispatchLogDto>> GetListAsync(GetListDispatchLogInput input)
    {
        var query = await _dispatchLogRepository.GetQueryableAsync();

        if (input.OrderId.HasValue)
        {
            query = query.Where(x => x.SourceOrderId == input.OrderId.Value || x.TargetOrderId == input.OrderId.Value);
        }

        if (input.PassengerId.HasValue)
        {
            query = query.Where(x => x.PassengerId == input.PassengerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(input.OperatorName))
        {
            query = query.Where(x => x.OperatorName != null && x.OperatorName.Contains(input.OperatorName));
        }

        if (input.StartTime.HasValue)
        {
            query = query.Where(x => x.CreationTime >= input.StartTime.Value);
        }

        if (input.EndTime.HasValue)
        {
            query = query.Where(x => x.CreationTime <= input.EndTime.Value);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);
        var items = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).PageBy(input.SkipCount, input.MaxResultCount));

        return new PagedResultDto<DispatchLogDto>(totalCount, ObjectMapper.Map<System.Collections.Generic.List<DispatchLog>, System.Collections.Generic.List<DispatchLogDto>>(items));
    }
}
