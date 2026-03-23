using Blazorise;
using Blazorise.DataGrid;
using CheMa.Go.Applications.AppServices;
using CheMa.Go.Applications.Dtos;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheMa.Go.Blazor.Components.Pages;

public partial class DispatchLogPage
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    [Inject]
    protected IDispatchLogAppService AppService { get; set; } = null!;

    protected GetListDispatchLogInput GetListInput { get; set; } = new() { MaxResultCount = 10 };

    protected IReadOnlyList<DispatchLogDto> Logs { get; set; } = new List<DispatchLogDto>();

    protected int PageSize { get; set; } = 10;

    protected int TotalCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SearchEntityAsync();
    }

    private async Task SearchEntityAsync()
    {
        GetListInput.SkipCount = 0;
        var result = await AppService.GetListAsync(GetListInput);
        Logs = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<DispatchLogDto> args)
    {
        GetListInput.SkipCount = (args.Page - 1) * args.PageSize;
        GetListInput.MaxResultCount = args.PageSize;
        var result = await AppService.GetListAsync(GetListInput);
        Logs = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    public void Dispose()
    {
        PageLayout.ShowToolbar = true;
    }
}
