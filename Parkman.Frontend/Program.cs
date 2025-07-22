using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Parkman.Frontend.Services;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.Bootstrap;

namespace Parkman.Frontend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddBlazorise(options => { options.Immediate = true; })
            .AddBootstrapProviders()
            .AddBootstrapIcons();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<ApiAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>());
        builder.Services.AddScoped<AuthService>();

        var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? builder.HostEnvironment.BaseAddress;
        builder.Services.AddScoped<IncludeCredentialsHandler>();
        builder.Services.AddScoped(sp =>
        {
            var handler = new IncludeCredentialsHandler
            {
                InnerHandler = new HttpClientHandler()
            };

            return new HttpClient(handler) { BaseAddress = new Uri(apiBaseAddress) };
        });

        await builder.Build().RunAsync();
    }
}
