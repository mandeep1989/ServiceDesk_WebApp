using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class ChangePasswordRequest
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public long? UserId { get; set; }
        public string CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public long IsDeleted { get; set; }
        public long? Status { get; set; }
        public string ApiTicketId { get; set; }
    }
}
