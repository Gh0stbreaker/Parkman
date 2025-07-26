using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Parkman.Frontend.ViewModels;

public class ConfirmEmailViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    private string _message = "Processing...";
    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public ConfirmEmailViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    public async Task InitializeAsync(string uri)
    {
        var absolute = _navigation.ToAbsoluteUri(uri);
        var query = QueryHelpers.ParseQuery(absolute.Query);
        if (query.TryGetValue("userId", out var userId) && query.TryGetValue("token", out var token))
        {
            var url = $"api/auth/confirm?userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(token)}";
            HttpResponseMessage response;
            try
            {
                response = await _http.GetAsync(url);
            }
            catch (HttpRequestException)
            {
                _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
                return;
            }
            Message = response.IsSuccessStatusCode ? "Email confirmed." : "Confirmation failed.";
        }
        else
        {
            Message = "Invalid confirmation link.";
        }
    }
}
