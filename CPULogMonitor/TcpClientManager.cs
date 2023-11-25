using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

public class TcpClientManager
{
    private readonly Logger _logger;

    public bool Connected;
    public bool Listening;
    public double SensorInterval = 6000;
    public string _address = "127.0.0.1";

    public TcpClientManager(Logger logger)
    {
        _logger = logger;
    }
    public void Start()
    {
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect(_address);
        }).Start();
    }

    public void Connect(String server)
    {
        try
        {
            Int32 port = 12345;
            TcpClient client = new TcpClient(server, port);

            Task.Run(() => ReceiveDataFromServer(client));

            Task.Run(() => SendDataToServer(client));
        }
        catch (Exception e)
        {
            _logger.WriteToFile($"{DateTime.Now}: Exception: {e}");
        }
    }

    private void ReceiveDataFromServer(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] bytes = new byte[256];

            while (true)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                }
                catch (IOException ex)
                {
                    if (client.Client != null && client.Client.Connected)
                    {
                        _logger.WriteToFile($"{DateTime.Now}: IOException while reading data: {ex.Message}");
                    }
                    else
                    {
                        _logger.WriteToFile($"{DateTime.Now}: Connection closed by the server.");
                        break;
                    }
                }

                string responseData = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                _logger.WriteToFile($"{DateTime.Now}: Received from server: {responseData}");

                string[] parts = responseData.Split(':');
                if (parts.Length >= 2)
                {
                    string command = parts[0];
                    double value = double.Parse(parts[1]);
                    if (command == "SET_SENSOR")
                    {
                        _logger.WriteToFile($"{DateTime.Now}: Command recived: {command}:{value}");
                        SetSensorTimer(value);
                    }
                    else
                    {
                        _logger.WriteToFile($"{DateTime.Now}: Unexpected message from server: {responseData}");
                    }

                }
            }
        }
        catch (Exception e)
        {
            _logger.WriteToFile($"{DateTime.Now}: Exception: {e}");
            client.Close();
        }
    }

    private void SendDataToServer(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                string request = GetCPUData();
                byte[] data = Encoding.ASCII.GetBytes(request);
                stream.Write(data, 0, data.Length);
                _logger.WriteToFile($"{DateTime.Now}: Sent to server: {request}");

                Thread.Sleep(6000);
            }
        }
        catch (Exception e)
        {
            _logger.WriteToFile($"{DateTime.Now}: Exception: {e}");
            client.Close();
        }
    }
    private string GetCPUData()
    {
        string filePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), "CPUData.json");
        string jsonData = "";

        if (File.Exists(filePath))
        {
            jsonData = File.ReadAllText(filePath);
            _logger.WriteToFile($"{DateTime.Now}: Sent CPU data to the server.");

            File.Move(filePath, Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), $"CPUData{DateTime.Now.ToString("yyyyMMddHHmmss")}.json"));
        }
        else
        {
            _logger.WriteToFile($"{DateTime.Now}: No data to send");
        }
        return jsonData;
    }
    private void SetSensorTimer(double value)
    {
        SensorInterval = value;
        _logger.WriteToFile($"{DateTime.Now}: Sensor Timer is set to {value}.");
    }

}