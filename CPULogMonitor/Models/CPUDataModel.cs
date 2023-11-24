using System;

namespace CPULogMonitor.Models
{
    public class CPUDataModel
    {
        public ClientModel ClientModel { get; set; }
        public int Id { get; set; }
        public string Ip { get; set; }
        public float? Temperature { get; set; }
        public float? Load { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
