using CASH.FLOW.TRACKER.API.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace CASH.FLOW.TRACKER.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var s = _config.GetSection("BREVOEMAIL");
            using var client = new SmtpClient(s["HOST"], int.Parse(s["PORT"]!))
            {
                Credentials = new NetworkCredential(s["USER"], s["PW"]),
                EnableSsl = true
            };
            var msg = new MailMessage(s["FROM"]!, to, subject, htmlBody) { IsBodyHtml = true };
            await client.SendMailAsync(msg);
        }
    }
}
