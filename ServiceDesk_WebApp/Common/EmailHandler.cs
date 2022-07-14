using System.Net.Mail;
using System.Text;

namespace ServiceDesk_WebApp.Common
{
    public static  class EmailHandler
    {
        private const string senderEmail = "bhatmohsin312@gmail.com";
        private const string senderPassword = "ppnicnotxqgjhlhn";

        public static async Task SendUserDetails( string password, string Email, string link)
        {
            MailMessage mail = new MailMessage(senderEmail, Email);
            mail.Subject = "Your user details with ServiceDesk APP is specified below";
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("Your Account Details with ServiceDesk APP is mention below <br/><br/>");
            sbBody.Append("Email : " + Email + "<br/><br/>");
            sbBody.Append("Password : " + password + "<br/><br/>");
            sbBody.Append("This password is insecure please change your password when ever you login <br/><br/>");
            sbBody.Append($"Click on the following link to login with <a href='{link}'  >ServiceDesk </a> <br/><br/>");

            sbBody.Append("Best Regarding Admin ServiceDesk APP");

            mail.Body = sbBody.ToString();
            mail.IsBodyHtml = true;


            SmtpClient client = new SmtpClient("smtp.googlemail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
            client.EnableSsl = true;
            await client.SendMailAsync(mail);

        }

    }
}
