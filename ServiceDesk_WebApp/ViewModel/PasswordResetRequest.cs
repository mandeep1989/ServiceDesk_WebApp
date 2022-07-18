namespace ServiceDesk_WebApp.ViewModel
{
    public class PasswordResetRequest
    {
        public string TicketId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long? Status { get; set; }


    }
}
