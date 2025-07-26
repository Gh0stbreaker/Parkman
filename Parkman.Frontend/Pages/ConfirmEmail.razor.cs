using Microsoft.AspNetCore.Components;
using Parkman.Frontend.ViewModels;

namespace Parkman.Frontend.Pages;

public partial class ConfirmEmail : MvvmComponent<ConfirmEmailViewModel>
{
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        return VM.InitializeAsync(Nav.Uri);
    }
}
