namespace ServiceDesk_WebApp.ViewModel
{
    public class ChangePasswordRequestModel
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// TicketId
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// ApiTicketId
        /// </summary>
        public string ApiTicketId { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
