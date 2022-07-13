using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class Vendor
    {
        public long UserId { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string ResidencyStatus { get; set; }
        public string Poremarks { get; set; }
        public string Currency { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public long IsDeleted { get; set; }
    }
}
