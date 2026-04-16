
namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}
