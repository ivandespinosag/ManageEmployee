using Microsoft.Extensions.Hosting;

namespace ManageEmployees.Services
{
    public class NotificationHostedService : IHostedService, IDisposable
    {
        private readonly INotificationService _notificationService;
        private Timer _timer;

        public NotificationHostedService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1)); // Ejecutar diariamente
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _notificationService.NotifyContractEnd();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
