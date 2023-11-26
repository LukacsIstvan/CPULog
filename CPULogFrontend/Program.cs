using CPULogFrontend.Providers;
using CPULogFrontend.Services.AuthService;
using CPULogFrontend.Services.ClientService;
using CPULogFrontend.Services.CpuDataService;
using CPULogFrontend.Services.ServerService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CPULogFrontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var address = "https://localhost:44381"; //Change this in production
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(address)
            });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            builder.Services.AddScoped<ICPUDataService, CPUDataService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IServerService, ServerService>();
            builder.Services.AddScoped<AuthService>();

            await builder.Build().RunAsync();
        }
    }
}
