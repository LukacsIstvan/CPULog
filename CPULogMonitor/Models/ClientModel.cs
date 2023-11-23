using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULogMonitor.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double SensorTimer { get; set; }
        public double SenderTimer { get; set; }
    }
}
