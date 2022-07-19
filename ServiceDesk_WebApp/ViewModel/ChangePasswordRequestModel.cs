namespace ServiceDesk_WebApp.ViewModel
{
    public class ChangePasswordRequestModel
    {
        public long UserId { get; set; }
        public string TicketId { get; set; }
        public string ApiTicketId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
