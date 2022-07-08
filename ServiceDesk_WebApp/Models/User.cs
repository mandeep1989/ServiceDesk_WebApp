using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long UserRole { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public long IsDeleted { get; set; }
    }
}
