using Microsoft.AspNetCore.Components;
using Parkman.Frontend.ViewModels;

namespace Parkman.Frontend.Pages;

public partial class Dashboard : MvvmComponent<DashboardViewModel>
{
    protected override Task OnInitializedAsync()
    {
        return VM.InitializeAsync();
    }
}
