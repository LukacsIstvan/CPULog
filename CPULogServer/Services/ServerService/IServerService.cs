using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using CPULogServer.Data;
using CPULogServer.Models;

namespace CPULogServer.Services.ServerService
{
    public interface IServerService
    {
        Task StartServer();
    }

}
