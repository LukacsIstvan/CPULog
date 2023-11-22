using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using CPULogServer.Models;
using System.IO;
using System.Text;

namespace CPULogServer.Services.ServerService
{
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> _logger;
        private readonly TcpListener _tcpListener;

        public ServerService(ILogger<ServerService> logger )
        {
            _logger = logger;
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
        }

        public async Task HandleClientCommunication(TcpClient client)
        {
            try
            {
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    while (true)
                    {
                        string json = await reader.ReadLineAsync();
                        if (string.IsNullOrEmpty(json))
                            break;

                        var cpuData = JsonConvert.DeserializeObject<CPUDataModel>(json);

                        _logger.LogInformation($"Received CPU Data from TCP: Temperature = {cpuData.Temperature}, Load = {cpuData.Load}");

                        //Store Data here
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in HandleClientCommunicationAsync: {ex.Message}");
            }
            finally
            {
                client.Close();
                _logger.LogInformation($"Client disconnected via TCP.");
            }
        }
        public async Task StartServer()
        {
            try
            {
                _tcpListener.Start();
                _logger.LogInformation($"Server is listening!");

                while (true)
                {
                    TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    _logger.LogInformation($"Client connected!");

                    await HandleClientCommunication(tcpClient);
                }
            }
            catch (SocketException ex)
            {
                _logger.LogInformation($"SocketException: {ex.SocketErrorCode}, {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception: {ex.Message}");
            }
            finally
            {
                _tcpListener.Stop();
                _logger.LogInformation("Server stopped.");
            }
        }

        
    }
}
