namespace ServiceDesk_WebApp.ViewModel
{
    public class PasswordResetRequest
    {
        /// <summary>
        /// TicketId
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// ApiTicketId
        /// </summary>
        public string ApiTicketId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public long? Status { get; set; }


    }
}
