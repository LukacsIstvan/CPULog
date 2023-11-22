using System;

namespace CPULogMonitor.Models
{
    public class CPUDataModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public float? Temperature { get; set; }
        public float? Load { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
