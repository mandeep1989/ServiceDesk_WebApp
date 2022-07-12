namespace ServiceDesk_WebApp.ViewModel
{
    public class LoginRequest
    {
        public string  Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {

        public long Id { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// UserRole
        /// </summary>
        public long UserRole { get; set; }

      

    }
    


}
