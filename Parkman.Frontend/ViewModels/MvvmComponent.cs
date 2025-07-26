using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Parkman.Frontend.ViewModels;

public class MvvmComponent<TViewModel> : ComponentBase, IDisposable where TViewModel : ViewModelBase
{
    [Inject] public TViewModel VM { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        VM.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        VM.PropertyChanged -= OnViewModelPropertyChanged;
    }
}
