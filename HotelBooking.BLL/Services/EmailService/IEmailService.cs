namespace HotelBooking.BLL.Services.EmailService
{
    public interface IEmailService
    {
        // Метод для відправки електронного листа
        Task SendMailAsync(string to, string subject, string body, bool isHtml);
    }
}
