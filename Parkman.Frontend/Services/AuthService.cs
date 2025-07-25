using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Parkman.Frontend.Models;

namespace Parkman.Frontend.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ApiAuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, ApiAuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<(bool Success, string? Error)> Login(string email, string password)
    {
        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
        }
        catch (HttpRequestException)
        {
            return (false, "Unable to reach the server.");
        }

        if (response.IsSuccessStatusCode)
        {
            _authStateProvider.NotifyAuthenticationStateChanged();
            return (true, null);
        }

        string? message = null;
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            message = problem?.Title ?? problem?.Detail;
        }
        catch { }

        if (string.IsNullOrWhiteSpace(message))
        {
            message = response.StatusCode switch
            {
                System.Net.HttpStatusCode.Forbidden => "Access denied.",
                System.Net.HttpStatusCode.Unauthorized => "Invalid email or password.",
                _ => "Login failed."
            };
        }

        return (false, message);
    }

    public async Task Logout()
    {
        try
        {
            var response = await _http.PostAsync("api/auth/logout", null);
            if (response.IsSuccessStatusCode)
            {
                _authStateProvider.NotifyAuthenticationStateChanged();
            }
        }
        catch (HttpRequestException)
        {
            // ignore when server is unreachable
        }
    }
}
