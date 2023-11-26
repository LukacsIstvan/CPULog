using System.Net;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CPULogServer.Models;
using System.Text;
using System.Collections.Generic;
using CPULogServer.Data;
using Microsoft.EntityFrameworkCore;

namespace CPULogServer.Services.ServerService
{
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> _logger;
        private readonly TcpListener _tcpListener = null;
        private List<TcpClient> _connectedClients;

        public ServerService(ILogger<ServerService> logger)
        {
            _logger = logger;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            _tcpListener = new TcpListener(localAddr, 12345);
            _connectedClients = new List<TcpClient>();
        }

        public Task StartServer()
        {
            Task.Run(() =>
            {
                _tcpListener.Start();
                StartListener();
            });

            _logger.LogInformation("Server Started!");
            return Task.CompletedTask;
        }
        private void StartListener()
        {
            try
            {
                while (true)
                {
                    _logger.LogInformation("Waiting for a connection...");

                    TcpClient client = _tcpListener.AcceptTcpClient();
                    _connectedClients.Add(client);
                    _logger.LogInformation($"Client conected!");

                    IPAddress clientIpAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    Client model = new Client();
                    model.Ip = clientIpAddress.ToString();
                    Task.Run(() => StoreClientData(model));

                    Task.Run(() => ReciveData(client));

                    Task.Run(() => SendSensorRequest(client));
                }
            }
            catch (SocketException e)
            {
                _logger.LogError($"SocketException: {e}");
                _tcpListener.Stop();
            }
        }

        private Task ReciveData(TcpClient client)
        {
            Task.Delay(60000);
            var stream = client.GetStream();

            string data = null;
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    ProcessData(data, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                    string str = "RECIVED";
                    Byte[] reply = Encoding.ASCII.GetBytes(str);
                    stream.Write(reply, 0, reply.Length);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                client.Close();
            }
            return Task.CompletedTask;
        }

        private void SendDataToClient(TcpClient client, string data)
        {
            try
            {
                if (client.Client != null && client.Client.Connected)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] bytes = Encoding.ASCII.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);
                    _logger.LogInformation("Sent to client: ", data);
                }
                else
                {
                    _logger.LogInformation("Client is not connected. Cannot send data.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
            }
        }

        private async Task SendSensorRequest(TcpClient client)
        {
            double SensorTimer = 60000;
            if (!client.Connected)
            {
                using (var context = new DataContext(new DbContextOptions<DataContext>()))
                {
                    string clientIpAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    Client dbClient = await context.Clients.FirstAsync(c => c.Ip == clientIpAddress);
                    SensorTimer = dbClient.SensorTimer;
                }

                while (true)
                {
                    string request = $"SET_SENSOR:{SensorTimer}";
                    SendDataToClient(client, request);
                    Task.Delay(3000);
                    
                }
            }
        }

        private async Task StoreClientData(Client client)
        {
            using (var context = new DataContext(new DbContextOptions<DataContext>()))
            {
                bool clientExists = await context.Clients.AnyAsync(c => c.Ip == client.Ip);

                if (!clientExists)
                {
                    client.SensorTimer = 6000;
                    await context.AddAsync(client);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"Client stored");
                }
                else
                {
                    _logger.LogInformation($"Client with IP {client.Ip} already exists.");
                }
            }
        }

        private async void ProcessData(string jsonData, string ip)
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    using (var context = new DataContext(new DbContextOptions<DataContext>()))
                    {
                        List<CPUData> cpuData = JsonConvert.DeserializeObject<List<CPUData>>(jsonData);
                        foreach (var cpu in cpuData)
                        {
                            Client client = await context.Clients.FirstAsync(c => c.Ip == ip);
                            cpu.ClientModel = client;
                            cpu.Ip = client.Ip;
                            await StoreCPUData(cpu, context);
                            _logger.LogInformation($"CPU Data: Temperature = {cpu.Temperature}, Load = {cpu.Load}");
                        }
                    }
                }
                catch (JsonException e)
                {
                    _logger.LogError($"JsonException:  {e}");
                }
            }
            else _logger.LogInformation($"Recived data is null");
        }

        private async Task StoreCPUData(CPUData data, DataContext context)
        {
            await context.CPUData.AddAsync(data);
            await context.SaveChangesAsync();
            _logger.LogInformation($"CPUData stored!");
        }
    }
}
