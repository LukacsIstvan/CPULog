using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using CPULogServer.Models;

namespace CPULogServer.Services.ServerService
{
    public interface IServerService
    {
        Task StartServer();
        Task HandleClientCommunication(TcpClient client);

    }

}
