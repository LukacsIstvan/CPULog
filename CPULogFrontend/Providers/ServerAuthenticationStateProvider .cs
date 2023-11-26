using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace CPULogFrontend.Providers
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState _authState;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_authState);
        }

        public void SetAuthenticationState(AuthenticationState authState)
        {
            _authState = authState;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
