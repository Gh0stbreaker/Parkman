using Microsoft.AspNetCore.Components;
using Parkman.Frontend.ViewModels;

namespace Parkman.Frontend.Pages;

public partial class ResetPassword : MvvmComponent<ResetPasswordViewModel>
{
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        VM.Initialize(Nav.Uri);
        return Task.CompletedTask;
    }
}
