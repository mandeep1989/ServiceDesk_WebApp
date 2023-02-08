using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class PaymentRequest
    {
        public long Id { get; set; }
        public string ContractTitle { get; set; }
        public string Department { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Classification { get; set; }
        public string ApplicationName { get; set; }
        public string ContractRefType { get; set; }
        public string ProjectName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string Details { get; set; }
        public byte[] OrginalInvoice { get; set; }
        public byte[] ServiceConfirmation { get; set; }
        public byte[] CopyOfApproval { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Iban { get; set; }
        public string SwiftCode { get; set; }
        public string Branch { get; set; }
        public string PaymentMode { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public long IsDeleted { get; set; }
        public string Ticketid { get; set; }
        public string Contract { get; set; }
        public string Vatamount { get; set; }
    }
}
