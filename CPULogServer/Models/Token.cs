using System;

namespace CPULogServer.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public User User { get; set; }
    }
}
