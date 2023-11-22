using OpenHardwareMonitor.Hardware;
using System.Net.Sockets;
using System.Net;
using System;
using CPULogMonitor.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class DataCollector
{
    private readonly Computer _computer;
    private readonly Logger _logger;

    public DataCollector(Logger logger)
    {
        _logger = logger;
        _computer = new Computer();
        _computer.CPUEnabled = true;
        _computer.Open();
    }
    private void AppendDataToJsonFile(CPUDataModel data)
    {
        try
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string filePath = Path.Combine(directoryPath, $"CPUData.json");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var jsonData = "";
            if (File.Exists(filePath))
            {
                jsonData = System.IO.File.ReadAllText(filePath);
            }

            var dataList = JsonConvert.DeserializeObject<List<CPUDataModel>>(jsonData) ?? new List<CPUDataModel>();
            dataList.Add(data);
            jsonData = JsonConvert.SerializeObject(dataList);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            _logger.WriteToFile($"{DateTime.Now}: Error appending data to JSON file: {ex.Message}");
        }
    }

    public CPUDataModel CollectData()
    {
        _logger.WriteToFile($"{DateTime.Now}: Collecting data from CPU...");

        CPUDataModel data = new CPUDataModel();
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        string address = "";
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                address = ip.ToString();
            }
        }
        data.Address = address;
        data.Temperature = GetCPUTemperature();
        data.Load = GetCPULoad();
        data.Timestamp = DateTime.Now;

        _logger.WriteToFile($"{DateTime.Now}: CPU data collected: {data.Address}, {data.Temperature}, {data.Load}, ");

        AppendDataToJsonFile(data);

        return data;
    }

    private float? GetCPUTemperature()
    {
        _logger.WriteToFile($"{DateTime.Now}: Collecting temperature data...");
        _computer.Hardware[0].Update();
        foreach (var sensor in _computer.Hardware[0].Sensors)
        {
            if (sensor.SensorType == SensorType.Temperature)
            {
                _logger.WriteToFile($"{DateTime.Now}: Sensor found!");
                return sensor.Value;
            }
        }
        _logger.WriteToFile($"{DateTime.Now}: Error: No temperature sensor found!");
        return null;
    }

    private float? GetCPULoad()
    {
        _logger.WriteToFile($"{DateTime.Now}: Collecting load data...");
        _computer.Hardware[0].Update();
        foreach (var sensor in _computer.Hardware[0].Sensors)
        {
            if (sensor.SensorType == SensorType.Load)
            {
                _logger.WriteToFile($"{DateTime.Now}: Sensor found!");
                return sensor.Value;
            }
        }
        _logger.WriteToFile($"{DateTime.Now}: Error: No load sensor found!");
        return null;
    }
}
