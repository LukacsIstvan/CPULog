using CPULogFrontend.Models;
using CPULogFrontend.Services.ClientService;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.ServerService
{
    public class ServerService : IServerService
    {
        private readonly HttpClient _httpClient;
        public ServerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task StartServer()
        {
            await _httpClient.GetAsync("api/Server/start");
        }
    }
}
