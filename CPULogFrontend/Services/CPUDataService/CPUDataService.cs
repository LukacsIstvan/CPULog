using CPULogFrontend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.CpuDataService
{
    public class CPUDataService : ICPUDataService
    {
        public List<CPUDataVM> CPUData { get; set; }
        private readonly HttpClient _httpClient;
        public CPUDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Get()
        {
            List<CPUDataVM> result = await _httpClient.GetFromJsonAsync<List<CPUDataVM>>("api/CPUData/get");
            CPUData = result;
        }

        public async Task GetByClient(int id)
        {
            List<CPUDataVM> result = await _httpClient.GetFromJsonAsync<List<CPUDataVM>>($"api/CPUData/get/client/{id}");
            CPUData = result;
        }
    }
}
