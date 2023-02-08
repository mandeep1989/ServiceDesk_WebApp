using System;
using System.Collections.Generic;

namespace ServiceDesk_WebApp.Models
{
    public partial class Contract
    {
        public long? Id { get; set; }
        public long ContractId { get; set; }
        public long? ParentContractId { get; set; }
        public string Name { get; set; }
        public string ContractType { get; set; }
        public string ContractClassification { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long Vendor { get; set; }
        public string MemoReference { get; set; }
        public long BudgetAmount { get; set; }
        public long Pid { get; set; }
        public string ProjectName { get; set; }
        public string CostCenter { get; set; }
        public string CostCenter2 { get; set; }
        public long? Pid2 { get; set; }
        public string CostCenter3 { get; set; }
        public long? Pid3 { get; set; }
        public string CostCenter4 { get; set; }
        public long? Pid4 { get; set; }
        public long Department { get; set; }
        public string Currency { get; set; }
        public double YearlyContractCostWithoutVat { get; set; }
        public string CostBreakdown { get; set; }
        public string CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long IsDeleted { get; set; }
        public byte[] BudgetAttchment { get; set; }
        public byte[] ContractAttachment { get; set; }
        public byte[] OtherAttchment { get; set; }
    }
}
