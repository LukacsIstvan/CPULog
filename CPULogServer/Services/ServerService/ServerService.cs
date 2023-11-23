using System.Net;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CPULogServer.Models;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CPULogServer.Services.ServerService
{
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> _logger;
        private readonly TcpListener _tcpListener;
        private List<TcpClient> _connectedClients;

        public ServerService(ILogger<ServerService> logger )
        {
            _logger = logger;
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
            _connectedClients = new List<TcpClient>();
        }

        public async Task GetCPUData(ClientModel client) 
        {
            TcpClient tcpClient = _connectedClients.Find(c => 
                ((IPEndPoint)c.Client.RemoteEndPoint).Address.ToString()==client.Ip);

            string json = await SendRequest(tcpClient, "REQUEST_CPU_DATA",0);

            await DeserializeIntoCPUData(json);
        }

        public async Task SetSensorTimer(ClientModel client, double value)
        {
            TcpClient tcpClient = _connectedClients.Find(c =>
                ((IPEndPoint)c.Client.RemoteEndPoint).Address.ToString() == client.Ip);

            string response = await SendRequest(tcpClient, "SET_SENSOR_TIMER", value);

            _logger.LogInformation($"Recived message from client {response}");
        }

        public async Task<List<CPUDataModel>> DeserializeIntoCPUData(string jsonData)
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    var cpuData = JsonConvert.DeserializeObject<List<CPUDataModel>>(jsonData);
                    foreach (var cpu in cpuData)
                    {
                        _logger.LogInformation($"CPU Data: Temperature = {cpu.Temperature}, Load = {cpu.Load}");
                    }
                    return cpuData;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"JsonException in DeserializeIntoCPUData: {ex.Message}");
                    return null;
                }
            }
            else _logger.LogInformation($"Recived data is null");
            return null;
        }

        public async Task<string> SendRequest(TcpClient client, string command, double value)
        {
            try
            {
                if (client == null)
                {
                    _logger.LogError("TcpClient is null.");
                    return null;
                }
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    await writer.WriteLineAsync($"{command}:{value}");
                    _logger.LogInformation($"Sent request for CPU data to the client.");

                    string jsonData = await reader.ReadLineAsync();

                    return jsonData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in SendRequest: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
            return null;
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

                    ClientModel client = new ClientModel();
                    client.Ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
                    _connectedClients.Add(tcpClient);
                    _logger.LogInformation($"Client connected: {client.Ip}");
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
