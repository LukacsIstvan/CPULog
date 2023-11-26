using CPULogFrontend.Models;
using CPULogFrontend.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.AuthService
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _jsRuntime = jsRuntime;
            Initialize();
        }

        private async void Initialize()
        {
            var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> Login(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                await UpdateAuthState(token);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> Register(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public async void Logout()
        {
            await UpdateAuthState(null);
        }

        private async Task UpdateAuthState(string token)
        {
            var identity = new ClaimsIdentity();

            if (token != null)
            {
                identity = ParseToken(token);
            }

            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            ((ServerAuthenticationStateProvider)_authStateProvider).SetAuthenticationState(authState);
        }

        private ClaimsIdentity ParseToken(string token)
        {            
            var claims = new[] { new Claim(ClaimTypes.Name, "username") };
            return new ClaimsIdentity(claims, "jwt");
        }

    }
}

