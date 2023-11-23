using System;
using System.Net.Sockets;

namespace CPULogServer.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public double SensorTimer { get; set; }
    }
}
