using System.Net;
using System.Net.Mail;
using System.Text;

namespace ServiceDesk_WebApp.Common
{
    public static  class EmailHandler
    {
        public static async Task SendUserDetails( string password, string Email, string link,string From,string SenderPassword,string host,int port)
        {
            MailMessage mail = new MailMessage(From, Email);
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
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            //System.Net.NetworkCredential basicCredential1 = new
            //System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.Credentials = basicCredential1;
            //    client.Send(mail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Credentials = new NetworkCredential(From, SenderPassword),
                UseDefaultCredentials = false,
                Timeout = 20000
            };
            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)

            {
                LogService log=new LogService();
                log.AddLogError(ex.Message + " " + ex.Message);
                
            }



        }

            public static async Task PasswordRequestMail(string TicketId , string ApiTicketId,string Email, string From, string SenderPassword, string host, int port)
        {
            MailMessage mail = new MailMessage(From, Email);
            mail.Subject = $" Request For Reset Password With App ID -- { TicketId  } and Service Desk Id --{ApiTicketId} Generated";
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append($"Your user Passowrd Change request with id { TicketId } has been sent to Admin <br/><br/>");
            sbBody.Append("Your Password Will Be Generated Shortly <br/><br/>");

            sbBody.Append("Best Regarding Admin ServiceDesk APP");

            mail.Body = sbBody.ToString();
            mail.IsBodyHtml = true;


            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(From, SenderPassword),
                Timeout = 20000
            };





            //SmtpClient client = new SmtpClient(host, port);
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
            await smtp.SendMailAsync(mail);

        }
        public static async Task PasswordResolveMail(string password, string TicketId,string ApiTicketId, string Email, string From, string SenderPassword, string host, int port,string link)
        {
            MailMessage mail = new MailMessage(From, Email);
            mail.Subject = $" Reset Password With ID -- { TicketId } Resolved";
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append($"Your user Passowrd Change request with id { TicketId } and Sevice Desk Plus Ticket no:{ApiTicketId} has been Resolved  <br/><br/>");
            sbBody.Append("Please Find Below Your Password <br/><br/>");
            sbBody.Append("Email : " + Email + "<br/><br/>");
            sbBody.Append("Password : " + password + "<br/><br/>");
            sbBody.Append($"Click on the following link to login with <a href='{link}'  >ServiceDesk </a> <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");
            mail.Body = sbBody.ToString();
            mail.IsBodyHtml = true;
            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(From, SenderPassword),
                Timeout = 20000
            };





            //SmtpClient client = new SmtpClient(host, port);
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
            await smtp.SendMailAsync(mail);

        }

    }
}
