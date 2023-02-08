namespace ServiceDesk_WebApp.ViewModel
{
    public class EscalationFormRequest
    {
        /// <summary>
        /// CompanyName
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// CompanyEmail
        /// </summary>
        public string CompanyEmail { get; set; }
        /// <summary>
        /// CompanyPhone
        /// </summary>
        public string CompanyPhone { get; set; }
        /// <summary>
        /// ContactName
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// ContactEmail
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// ContactPhone
        /// </summary>
        public string ContactPhone { get; set; }
		public string ManagerName { get; set; }
		public string ManagerEmail { get; set; }
		public string ManagerPhone { get; set; }
	}
}
