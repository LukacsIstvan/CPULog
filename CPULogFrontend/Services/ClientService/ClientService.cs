using CPULogFrontend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.ClientService
{
    public class ClientService : IClientService
    {
        public List<ClientVM> Clients { get; set; }
        private readonly HttpClient _httpClient;
        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Get()
        {
            List<ClientVM> result = await _httpClient.GetFromJsonAsync<List<ClientVM>>("api/client/get");
            Clients = result;
        }

        public async Task Get(int id)
        {
            List<ClientVM> result = await _httpClient.GetFromJsonAsync<List<ClientVM>>($"api/client/get/{id}");
            Clients = result;
        }
        public async Task SetSensor(int id,double value)
        {
            var request = new
            {
                id = id,
                value = value
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/client/timer", request);
        }
    }
}
