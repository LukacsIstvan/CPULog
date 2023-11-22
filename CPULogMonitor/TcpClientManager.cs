using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using CPULogMonitor.Models;
using Newtonsoft.Json;

public class TcpClientManager : IDisposable
{
    private TcpClient _tcpClient;
    private Logger _logger;

    public TcpClientManager(Logger fileWriter)
    {
        _logger = fileWriter;
    }

    public void ConnectToServer()
    {
        try
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect("localhost", 12345);
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

    public async Task SendDataToServer(CPUDataModel data)
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
            _logger.WriteToFile($"{DateTime.Now}: Client is not connected.");
            _logger.WriteToFile($"{DateTime.Now}: Reconnecting...");
            ConnectToServer();
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
