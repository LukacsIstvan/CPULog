using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULogMonitor.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public double SensorTimer { get; set; }
    }
}
