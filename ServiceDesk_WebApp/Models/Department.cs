using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class Department
    {
        public long Id { get; set; }
        public string DepartmentName { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long IsDeleted { get; set; }
    }
}
