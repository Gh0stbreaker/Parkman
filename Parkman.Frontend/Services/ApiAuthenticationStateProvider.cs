using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Parkman.Frontend.Services;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;

    public ApiAuthenticationStateProvider(HttpClient http)
    {
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var profile = await _http.GetFromJsonAsync<ProfileDto>("api/user/profile");
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, profile.Email) }, "serverAuth");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyAuthenticationStateChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    private class ProfileDto
    {
        public string Email { get; set; } = string.Empty;
    }
}
