using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;
using System.Text;
using MailKit.Net.Smtp;



namespace ServiceDesk_WebApp.Common
{
    public static  class EmailHandler
    {
        public static async Task SendUserDetails( string password, string Email, string link,string From,string SenderPassword,string host,int port,bool SSlEnable)
        {
            //MailMessage mail = new MailMessage(From, Email);
            //mail.Subject = "Your user details with ServiceDesk APP is specified below";
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("Your Account Details with ServiceDesk APP is mention below <br/><br/>");
            sbBody.Append("Email : " + Email + "<br/><br/>");
            sbBody.Append("Password : " + password + "<br/><br/>");
            sbBody.Append("This password is insecure please change your password when ever you login <br/><br/>");
            sbBody.Append($"Click on the following link to login with <a href='{link}'  >ServiceDesk </a> <br/><br/>");
            sbBody.Append("Best Regarding Admin ServiceDesk APP");

            //  mail.Body = sbBody.ToString();
            //   mail.IsBodyHtml = true;
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            //System.Net.NetworkCredential basicCredential1 = new
            //System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.Credentials = basicCredential1;
            //    client.Send(mail);

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential(From, SenderPassword),
            //    UseDefaultCredentials = false,
            //    Timeout = 20000
            //};


            //using (var client = new MailKit.Net.Smtp.SmtpClient())
            //{
            //    try
            //    {
            //        await client.ConnectAsync("smtp.gmail.com", 465, true);
            //        client.AuthenticationMechanisms.Remove("XOAUTH2");
            //        await client.AuthenticateAsync("dtx.softprodigy@gmail.com", "pnylgqlgauhqoaso");
            //        await client.SendAsync((MimeKit.MimeMessage)mail);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        await client.DisconnectAsync(true);
            //        client.Dispose();
            //    }
            //}



            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(From));
            email.To.Add(MailboxAddress.Parse(Email));
            email.Subject = "Hi";
            email.Body = new TextPart(TextFormat.Html) { Text = sbBody.ToString() };


            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(From, SenderPassword);
            smtp.Send(email);
            smtp.Disconnect(true);


            //try
            //{
            //    await smtp.SendMailAsync(mail);
            //}
            //catch (Exception ex)

            //{
            //    LogService log=new LogService();
            //    log.AddLogError(ex.Message + " " + ex.Message);

            //}

            //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //mail.To.Add("mirkamranellahi@gmail.com");
            //mail.From = new MailAddress("dtx.softprodigy@gmail.com", "Email head", System.Text.Encoding.UTF8);
            //mail.Subject = "This mail is send from asp.net application";
            //mail.SubjectEncoding = System.Text.Encoding.UTF8;
            //mail.Body = "This is Email Body Text";
            //mail.BodyEncoding = System.Text.Encoding.UTF8;
            //mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //client.Credentials = new System.Net.NetworkCredential("dtx.softprodigy@gmail.com", "pnylgqlgauhqoaso");
            //client.Port = 587;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = true;
            //try
            //{
            //    client.Send(mail);
            //    //Page.RegisterStartupScript("UserMsg", "<script>alert('Successfully Send...');if(alert){ window.location='SendMail.aspx';}</script>");
            //}
            //catch (Exception ex)
            //{
            //    Exception ex2 = ex;
            //    string errorMessage = string.Empty;
            //    while (ex2 != null)
            //    {
            //        errorMessage += ex2.ToString();
            //        ex2 = ex2.InnerException;
            //    }
            //}



















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


            //var smtp = new SmtpClient
            //{
            //    Host = host,
            //    Port = port,
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    Credentials = new NetworkCredential(From, SenderPassword),
            //    Timeout = 20000
            //};





            //SmtpClient client = new SmtpClient(host, port);
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
          //  await smtp.SendMailAsync(mail);

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
            //var smtp = new SmtpClient
            //{
            //    Host = host,
            //    Port = port,
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    Credentials = new NetworkCredential(From, SenderPassword),
            //    Timeout = 20000
            //};





            //SmtpClient client = new SmtpClient(host, port);
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(From, SenderPassword);
            //client.EnableSsl = true;
           // await smtp.SendMailAsync(mail);

        }

    }
}
