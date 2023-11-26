using CPULogFrontend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.ClientService
{
    public interface IClientService
    {
        public List<Client> Clients { get; set; }

        Task Get();
        Task Get(int id);
        Task SetSensor(int id, double value);
    }
}
