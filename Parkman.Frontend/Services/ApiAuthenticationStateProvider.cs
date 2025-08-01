using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, profile.Email) };
            if (profile.Roles != null)
            {
                claims.AddRange(profile.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }
            var identity = new ClaimsIdentity(claims, "serverAuth");
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
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? VehicleType { get; set; }
        public string? PropulsionType { get; set; }
        public List<string>? Roles { get; set; }
    }
}
