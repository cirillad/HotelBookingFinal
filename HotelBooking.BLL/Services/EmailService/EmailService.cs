using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        // Конструктор для ініціалізації SMTP-клієнта та адреси відправника
        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient(configuration["EmailSettings:Host"])
            {
                Port = int.Parse(configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]),
                EnableSsl = true
            };
            _fromAddress = configuration["EmailSettings:FromAddress"];
        }

        // Метод для відправки електронного листа
        public async Task SendMailAsync(string to, string subject, string body, bool isHtml)
        {
            var mailMessage = new MailMessage(_fromAddress, to, subject, body)
            {
                IsBodyHtml = isHtml
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
