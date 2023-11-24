using CPULogServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogServer.Services.CPUDataService
{
    public interface ICPUDataService
    {
        Task Store(CPUDataModel cpuData);
        Task<CPUDataModel> Get(int id);
        Task<List<CPUDataModel>> Get();
        Task Update(int id, CPUDataModel cpuData);
        Task Delete(int id);
    }
}
