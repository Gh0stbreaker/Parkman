using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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

    public async Task<bool> Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
        if (response.IsSuccessStatusCode)
        {
            _authStateProvider.NotifyAuthenticationStateChanged();
            return true;
        }
        return false;
    }

    public async Task Logout()
    {
        var response = await _http.PostAsync("api/auth/logout", null);
        if (response.IsSuccessStatusCode)
        {
            _authStateProvider.NotifyAuthenticationStateChanged();
        }
    }
}
