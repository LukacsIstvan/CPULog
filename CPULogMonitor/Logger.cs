using System.IO;
using System;

public class Logger
{
    private readonly string _filePath;

    public Logger(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteToFile(string message)
    {
        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filepath = Path.Combine(directoryPath, $"ServiceLog_{DateTime.Now:yyyyMMdd_HH}.txt");

        if (!File.Exists(filepath))
        {
            using (StreamWriter streamWriter = File.CreateText(filepath))
            {
                streamWriter.WriteLine(message);
            }
        }
        else
        {
            using (StreamWriter streamWriter = File.AppendText(filepath))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}
