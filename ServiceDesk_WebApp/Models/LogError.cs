using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class LogError
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Information { get; set; }
        public string Time { get; set; }
    }
}
