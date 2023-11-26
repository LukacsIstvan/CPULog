using CPULogServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogServer.Services.ClientService
{
    public interface IClientService
    {
        Task Store(Client cpuData);
        Task<Client> Get(int id);
        Task<List<Client>> Get();
        Task Update(int id, Client cpuData);
        Task Delete(int id);
        Task SetSensor(int id, double value);
    }
}
