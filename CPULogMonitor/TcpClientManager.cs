using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using CPULogMonitor.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.CodeDom;

public class TcpClientManager : IDisposable
{
    public TcpClient _tcpClient;
    private readonly Logger _logger;

    public bool Connected;
    public bool Listening;
    public double SensorInterval;

    public TcpClientManager(Logger logger)
    {
        _logger = logger;

        SensorInterval=6000;
        Listening = false;
        Connected = false;
    }
    public async void ListenForServerRequests()
    {
        if (Listening)
        {
            _logger.WriteToFile($"{DateTime.Now}: Already listening...");
            return;
        }

        Listening = true;
            while (_tcpClient.Connected)
            {
                try
                {
                    while (_tcpClient.Connected)
                    {
                        _logger.WriteToFile($"{DateTime.Now}: Listening to the server...");

                        Listening = true;
                        using (var stream = _tcpClient.GetStream())
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                        {
                            string request = await reader.ReadLineAsync();
                            string[] parts = request.Split(':');
                            string command = parts[0];
                            double value = double.Parse(parts[1]);

                            if (command == "REQUEST_CPU_DATA")
                            {
                                _logger.WriteToFile($"{DateTime.Now}: Command recived: {command}");

                                string filePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), "CPUData.json");
                                string jsonData = "";

                                if (File.Exists(filePath))
                                {
                                    jsonData = File.ReadAllText(filePath);
                                    await writer.WriteLineAsync(jsonData);
                                    _logger.WriteToFile($"{DateTime.Now}: Sent CPU data to the server.");

                                    File.Move(filePath, Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), $"CPUData{DateTime.Now.ToString("yyyyMMddHHmmss")}.json"));
                                    _logger.WriteToFile($"{DateTime.Now}: CPUData sent to server and file deleted: {filePath}");
                                }
                                else
                                {
                                    _logger.WriteToFile($"{DateTime.Now}: No data to send");
                                }
                                await writer.WriteLineAsync(jsonData);
                            }
                            else if (command == "SET_SENSOR_TIMER")
                            {
                                _logger.WriteToFile($"{DateTime.Now}: Command recived: {command}");
                                SensorInterval = value;
                                string message = "OK";
                                await writer.WriteLineAsync(message);
                                _logger.WriteToFile($"{DateTime.Now}: Sensor Timer is set to {value}.");
                            }
                            else
                            {
                                _logger.WriteToFile($"{DateTime.Now}: Unexpected message from server: {request}");
                            }
                        }
                    }
                    if (!_tcpClient.Connected)
                    {
                        Connected = false;
                        Listening = false;
                        _logger.WriteToFile($"{DateTime.Now}: Listening stopped!");
                    }

                }
                catch (Exception ex)
                {
                    _logger.WriteToFile($"{DateTime.Now}: Error: {ex.Message}");
                }
            }
            Listening = false;
            _logger.WriteToFile($"{DateTime.Now}: Listening stopped!");
    }

    public async void SendDataToServer(CPUDataModel data)
    {
        if (_tcpClient.Connected)
        {
            
            _logger.WriteToFile($"{DateTime.Now}: Sending data to server... load: {data.Load}, temp: {data.Temperature}");

            var json = JsonConvert.SerializeObject(data);

            try
            {
                using (var stream = _tcpClient.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    await writer.WriteLineAsync(json);
                    await writer.FlushAsync();
                }
            }
            catch (SocketException ex)
            {
                _logger.WriteToFile($"{DateTime.Now}: SocketException: {ex.SocketErrorCode}, {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.WriteToFile($"{DateTime.Now}: Error: Sending data to server failed. error: {ex.Message}");
            }
        }
        else
        {
            Connected = false;
            _logger.WriteToFile($"{DateTime.Now}: Client is not connected.");
        }
    }
    public void ConnectToServer()
    {
        try
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect("localhost", 12345);
            _logger.WriteToFile($"{DateTime.Now}: Client connected to the server");
            Connected = true;

            // Call ListenForServerRequests after connecting
            Task.Run(() => ListenForServerRequests());
        }
        catch (SocketException ex)
        {
            _logger.WriteToFile($"{DateTime.Now}: SocketException: {ex.SocketErrorCode}, {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.WriteToFile($"{DateTime.Now}: Exception: {ex.Message}");
        }
    }
    public void Dispose()
    {
        if (_tcpClient != null && _tcpClient.Connected)
        {
            _tcpClient.GetStream()?.Close();
            _tcpClient.Close();
        }
    }
}