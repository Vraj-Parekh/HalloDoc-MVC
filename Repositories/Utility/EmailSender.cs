using MimeKit;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;

namespace HalloDoc.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, List<string>? attachments = null)
        {
            MimeMessage? emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("vraj.parekh@etatvasoft.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;

            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = message;

            if (attachments != null)
            {
                foreach (string attachment in attachments)
                {
                    MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(attachment));
                    builder.Attachments.Add(attachment, memoryStream);
                }
            }
            emailToSend.Body = builder.ToMessageBody();
          
            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                emailClient.Connect("mail.etatvasoft.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("vraj.parekh@etatvasoft.com", "9WQPH4a!f0ew");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
                return Task.CompletedTask;
            }
        }

    }

}




