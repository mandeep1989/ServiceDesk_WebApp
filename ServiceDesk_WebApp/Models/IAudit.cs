using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk_WebApp.Models
{
    public interface IAudit
    {
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public long IsDeleted { get; set; }
    }

    public partial class User : IAudit { }
    public partial class Vendor : IAudit { }
}
