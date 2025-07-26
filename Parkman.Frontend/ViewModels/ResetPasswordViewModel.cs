using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Parkman.Shared.Models;

namespace Parkman.Frontend.ViewModels;

public class ResetPasswordViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    public ResetPasswordRequest Model { get; } = new();
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

    public ResetPasswordViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
        EditContext = new EditContext(Model);
    }

    public void Initialize(string uri)
    {
        var absolute = _navigation.ToAbsoluteUri(uri);
        var query = QueryHelpers.ParseQuery(absolute.Query);
        if (query.TryGetValue("email", out var e))
        {
            Model.Email = e;
        }
        if (query.TryGetValue("token", out var t))
        {
            Model.Token = t;
        }
    }

    public async Task HandleAsync()
    {
        IsSubmitting = true;
        Message = null;
        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/reset-password", Model);
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
            IsSubmitting = false;
            return;
        }

        Message = response.IsSuccessStatusCode ? "Password reset successful." : "Password reset failed.";
        IsSubmitting = false;
    }
}
