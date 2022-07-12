using System.ComponentModel.DataAnnotations;

namespace ServiceDesk_WebApp.Models
{
    public class Vendor
    {
        public long Id { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }

        [Display(Name ="Residency Status")]
        public string ResidencyStatus { get; set; }

        [Display(Name = "PO Remarks")]
        public string PORemarks { get; set; }

        public string Currency { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
