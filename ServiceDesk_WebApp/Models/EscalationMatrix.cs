using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class EscalationMatrix
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long IsDeleted { get; set; }
    }
}
