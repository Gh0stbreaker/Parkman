using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkman.Frontend.Services;
using Parkman.Shared.Models;

namespace Parkman.Frontend.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly AuthService _auth;
    private readonly NavigationManager _navigation;

    public LoginRequest Model { get; } = new();
    public EditContext EditContext { get; }

    private bool _showPassword;
    public bool ShowPassword
    {
        get => _showPassword;
        set => SetProperty(ref _showPassword, value);
    }

    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public LoginViewModel(AuthService auth, NavigationManager navigation)
    {
        _auth = auth;
        _navigation = navigation;
        EditContext = new EditContext(Model);
    }

    public async Task HandleLogin()
    {
        IsSubmitting = true;
        ErrorMessage = null;
        var result = await _auth.Login(Model.Email, Model.Password);
        if (result.Success)
        {
            _navigation.NavigateTo("/dashboard");
        }
        else
        {
            ErrorMessage = result.Error;
        }
        IsSubmitting = false;
    }
}
