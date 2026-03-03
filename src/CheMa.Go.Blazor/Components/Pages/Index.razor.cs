using Microsoft.AspNetCore.Components;

namespace CheMa.Go.Blazor.Components.Pages;

public partial class Index
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    protected override void Dispose(bool disposing)
    {
        PageLayout.ShowToolbar = true;
    }
}
