namespace ServiceDesk_WebApp.ViewModel
{
    public class PaymentRequestModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ContractTitle
        /// </summary>
        public string ContractTitle { get; set; }
        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Classification
        /// </summary>
        public string Classification { get; set; }
        /// <summary>
        /// ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// ContractRefType
        /// </summary>
        public string ContractRefType { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// InvoiceDate
        /// </summary>
        public string InvoiceDate { get; set; }
        /// <summary>
        /// InvoiceAmount
        /// </summary>
        public string InvoiceAmount { get; set; }
        /// <summary>
        /// Details
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// OrginalInvoice
        /// </summary>
        public byte[] OrginalInvoice { get; set; }
        /// <summary>
        /// ServiceConfirmation
        /// </summary>
        public byte[] ServiceConfirmation { get; set; }
        /// <summary>
        /// CopyOfApproval
        /// </summary>
        public byte[] CopyOfApproval { get; set; }
        /// <summary>
        /// BankName
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// AccountName
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Iban
        /// </summary>
        public string Iban { get; set; }
        /// <summary>
        /// SwiftCode
        /// </summary>
        public string SwiftCode { get; set; }
        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// PaymentMode
        /// </summary>
        public string PaymentMode { get; set; }

        //public DateOnly Created { get; set; }
        public DateTime Created { get; set; }
    }
}
