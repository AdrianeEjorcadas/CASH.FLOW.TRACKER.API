using CASH.FLOW.TRACKER.API.Services.Interface;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace CASH.FLOW.TRACKER.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var s = _config.GetSection("BREVOEMAIL");
            var apiKey = s["APIKEY"]!;
            var fromEmail = s["FROM"]!;

            var payload = new
            {
                sender = new { email = fromEmail },
                to = new[] { new { email = to } },
                subject = subject,
                htmlContent = htmlBody
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("api-key", apiKey);
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Brevo API error ({response.StatusCode}): {error}");
            }
        }

        //public async Task SendAsync(string to, string subject, string htmlBody)
        //{
        //    var s = _config.GetSection("BREVOEMAIL");
        //    using var client = new SmtpClient(s["HOST"], int.Parse(s["PORT"]!))
        //    {
        //        Credentials = new NetworkCredential(s["USER"], s["PW"]),
        //        EnableSsl = true
        //    };
        //    var msg = new MailMessage(s["FROM"]!, to, subject, htmlBody) { IsBodyHtml = true };
        //    await client.SendMailAsync(msg);
        //}
    }
}
