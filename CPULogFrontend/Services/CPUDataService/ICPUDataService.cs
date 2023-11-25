using CPULogFrontend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.CpuDataService
{
    public interface ICPUDataService
    {
        public List<CPUDataVM> CPUData { get; set; }

        Task Get();
        Task GetByClient(int id);
    }
}
