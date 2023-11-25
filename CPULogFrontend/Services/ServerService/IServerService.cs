using CPULogFrontend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPULogFrontend.Services.ServerService
{
    public interface IServerService
    {
        Task StartServer();
    }
}
