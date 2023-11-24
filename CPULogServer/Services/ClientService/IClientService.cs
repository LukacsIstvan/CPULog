using CPULogServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogServer.Services.ClientService
{
    public interface IClientService
    {
        Task Store(ClientModel cpuData);
        Task<ClientModel> Get(int id);
        Task<List<ClientModel>> Get();
        Task Update(int id, ClientModel cpuData);
        Task Delete(int id);
    }
}
