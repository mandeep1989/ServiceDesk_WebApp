using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;
using System.Text;
using MailKit.Net.Smtp;



namespace ServiceDesk_WebApp.Common
{
    public static class EmailHandler
    {
        public static async Task SendUserDetails(string password, string Email, string link, string From, string SenderPassword, string host, int port, bool SSlEnable)
        {

            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("Your Account Details with ServiceDesk APP is mention below <br/><br/>");
            sbBody.Append("Email : " + Email + "<br/><br/>");
            sbBody.Append("Password : " + password + "<br/><br/>");
            sbBody.Append("This password is insecure please change your password when ever you login <br/><br/>");
            sbBody.Append($"Click on the following link to login with <a href='{link}'  >ServiceDesk </a> <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(Email));
            email.Subject = "Account Details";
            email.Body = new TextPart(TextFormat.Html) { Text = sbBody.ToString() };
            // send email
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(host, port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(From, SenderPassword);
                    await client.SendAsync((MimeKit.MimeMessage)email);
                }
                catch (Exception ex)
                {

                   throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }

        }

        public static async Task PasswordRequestMail(string TicketId, string ApiTicketId, string Email, string From, string SenderPassword, string host, int port)
        {
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append($"Your user Passowrd Change request with id { TicketId } has been sent to Admin <br/><br/>");
            sbBody.Append("Your Password Will Be Generated Shortly <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(Email));
            email.Subject = $" Request For Reset Password With App ID -- { TicketId  } and Service Desk Id --{ApiTicketId} Generated";
            email.Body = new TextPart(TextFormat.Html) { Text = sbBody.ToString() };
            // send email
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(host, port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(From, SenderPassword);
                    await client.SendAsync((MimeKit.MimeMessage)email);
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }



        }
        public static async Task PasswordResolveMail(string password, string TicketId, string ApiTicketId, string Email, string From, string SenderPassword, string host, int port, string link)
        {

            StringBuilder sbBody = new StringBuilder();
            sbBody.Append($"Your user Passowrd Change request with id { TicketId } and Sevice Desk Plus Ticket no:{ApiTicketId} has been Resolved  <br/><br/>");
            sbBody.Append("Please Find Below Your Password <br/><br/>");
            sbBody.Append("Email : " + Email + "<br/><br/>");
            sbBody.Append("Password : " + password + "<br/><br/>");
            sbBody.Append($"Click on the following link to login with <a href='{link}'  >ServiceDesk </a> <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");
            sbBody.Append($"Your user Passowrd Change request with id { TicketId } has been sent to Admin <br/><br/>");
            sbBody.Append("Your Password Will Be Generated Shortly <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(Email));
            email.Subject = $" Reset Password With ID -- { TicketId } Resolved";

            email.Body = new TextPart(TextFormat.Html) { Text = sbBody.ToString() };
            // send email
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(host, port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(From, SenderPassword);
                    await client.SendAsync((MimeKit.MimeMessage)email);
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }

        }

    }
}
