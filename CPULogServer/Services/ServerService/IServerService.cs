using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using CPULogServer.Models;

namespace CPULogServer.Services.ServerService
{
    public interface IServerService
    {
        Task StartServer();
        Task<string> SendRequest(TcpClient client, string command, double value);
        Task<List<CPUDataModel>> DeserializeIntoCPUData(string jsonData);
        Task GetCPUData(ClientModel client);
        Task SetSensorTimer(ClientModel client, double interval);
    }

}
