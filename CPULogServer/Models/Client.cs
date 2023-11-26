using System;
using System.Net.Sockets;

namespace CPULogServer.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public double SensorTimer { get; set; }
    }
}
