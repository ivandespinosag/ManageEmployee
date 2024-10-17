namespace ManageEmployees.Services
{
    public interface INotificationService
    {
        Task NotifyContractEnd();
        Task SendNotificationEmail(string toEmail, string subject, string body);
    }
}
