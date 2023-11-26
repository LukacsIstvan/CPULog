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
        _computer = new Computer();
        _computer.CPUEnabled = true;
        _computer.Open();
        _logger = logger;
    }
    private void AppendDataToJsonFile(CPUData data)
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

            var dataList = JsonConvert.DeserializeObject<List<CPUData>>(jsonData) ?? new List<CPUData>();
            dataList.Add(data);
            jsonData = JsonConvert.SerializeObject(dataList);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            _logger.WriteToFile($"{DateTime.Now}: Error appending data to JSON file: {ex.Message}");
        }
    }

    public CPUData CollectData()
    {
        _logger.WriteToFile($"{DateTime.Now}: Collecting data from CPU...");

        CPUData data = new CPUData();
        data.Temperature = GetCPUTemperature();
        data.Load = GetCPULoad();
        data.Timestamp = DateTime.Now;

        _logger.WriteToFile($"{DateTime.Now}: CPU data collected: {data.Temperature}, {data.Load} ");

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
                return sensor.Value;
            }
        }
        _logger.WriteToFile($"{DateTime.Now}: Error: No temperature sensor found!");
        return null;
    }

    private float? GetCPULoad()
    {
        _computer.Hardware[0].Update();
        foreach (var sensor in _computer.Hardware[0].Sensors)
        {
            if (sensor.SensorType == SensorType.Load)
            {
                return sensor.Value;
            }
        }
        return null;
    }
}
