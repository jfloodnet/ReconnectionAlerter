using System.Net.Mail;
using System.Text;

namespace ReconnectionAlerter.Email
{
    public class SMTPMailSender
    {
        public void SendEmail(string from, string to, string subject, string body)
        {
            var smptClient = new SmtpClient();

            using (var message = CreateMailMessage(subject, body))
            {
                if (from != null) message.From = new MailAddress(from);

                message.To.Add(to.Replace(';', ','));

                smptClient.Send(message);
            }
        }

        private static MailMessage CreateMailMessage(string subject, string body)
        {
            return new MailMessage
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                };
        }
    }
}
