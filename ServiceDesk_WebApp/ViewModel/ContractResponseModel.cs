namespace ServiceDesk_WebApp.ViewModel
{
	public class ContractResponseModel
	{
		public long ContractId { get; set; }
		public string Name { get; set; }
		public string ContractType { get; set; }
		public string Vendor { get; set; }
		public string Department { get; set; }
		public long DepartmentId { get; set; }
		public string Description { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string ContractClassification { get; set; }
		public string ProjectName { get; set; }
	}
	public class GeneralDropdown
	{
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
