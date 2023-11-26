using System;

namespace CPULogFrontend.Models
{
    public class CPUData
    {

        public Client ClientModel { get; set; }
        public int Id { get; set; }
        public string Ip { get; set; }
        public float? Temperature { get; set; }
        public float? Load { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
