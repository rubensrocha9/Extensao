using GestorPay.Models.Service.Interface;
using MailKit.Net.Smtp;
using MimeKit;

namespace GestorPay.Models.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(Email email)
        {
            var emailMassage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMassage.From.Add(new MailboxAddress("Authentication", from));
            emailMassage.To.Add(new MailboxAddress(email.To, email.To));
            emailMassage.Subject = email.Subject;
            emailMassage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMassage);
                }
                catch (Exception)
                {

                    throw;
                }

                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
