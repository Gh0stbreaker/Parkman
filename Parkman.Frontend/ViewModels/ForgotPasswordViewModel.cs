using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkman.Shared.Models;

namespace Parkman.Frontend.ViewModels;

public class ForgotPasswordViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    public ForgotPasswordRequest Model { get; } = new();
    public EditContext EditContext { get; }

    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }

    private string? _message;
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ForgotPasswordViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
        EditContext = new EditContext(Model);
    }

    public async Task HandleAsync()
    {
        IsSubmitting = true;
        Message = null;
        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/forgot-password", Model);
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
            IsSubmitting = false;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Message = "If an account with that email exists, a reset link was sent.";
            Model.Email = string.Empty;
            EditContext.MarkAsUnmodified();
        }
        else
        {
            Message = "Request failed.";
        }
        IsSubmitting = false;
    }
}
