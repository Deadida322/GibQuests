using AuthService.Database.Interfaces;
using AuthService.Main.SettingModels;
using Microsoft.Extensions.Options;

namespace AuthService.Main.Services
{
    public class RemoveOldRefreshTokenService : BackgroundService
    {
        private IServiceProvider _services { get; }

        private RefreshTokenServiceSetting _setting;

        public RemoveOldRefreshTokenService(IServiceProvider services, IOptions<RefreshTokenServiceSetting> options)
        {
            _services = services;
            _setting = options.Value;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                            .GetRequiredService<IRefreshStorage>();

                    //удаляем старые токены
                    await scopedProcessingService.DeleteOldRefreshToken();
                }

                //получаем из конфигурации время в минутах и преобразовываем к мс
                await Task.Delay(60000 * _setting.RefreshTokenServicePeriodMinutes, stoppingToken);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }
    }
}
