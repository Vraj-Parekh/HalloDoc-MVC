using MimeKit;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;

namespace HalloDoc.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("vraj.parekh@etatvasoft.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };


            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                emailClient.Connect("mail.etatvasoft.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("vraj.parekh@etatvasoft.com", "9WQPH4a!f0ew");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
                return Task.CompletedTask;
            }

            //var mail = "vrajparekh58@gmail.com";
            //var password = "bhul lsjl twue amf";

            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(mail, password)
            //};

            //client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
            //return Task.CompletedTask;
        }
    }
}




