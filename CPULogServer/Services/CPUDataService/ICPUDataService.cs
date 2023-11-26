using CPULogServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogServer.Services.CPUDataService
{
    public interface ICPUDataService
    {
        Task Store(CPUData cpuData);
        Task<CPUData> Get(int id);
        Task<List<CPUData>> Get();
        Task Update(int id, CPUData cpuData);
        Task Delete(int id);
        Task<List<CPUData>> GetByClient(int id);
    }
}
