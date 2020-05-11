using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UniqueWords.WebApp.StartupConfigs
{
    public class AppLifetimeEventsHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<AppLifetimeEventsHostedService> _logger;
        private readonly IHostEnvironment _env;
        private readonly IHostApplicationLifetime _appLifetime;

        private CancellationTokenRegistration _appStartedRegistration;
        private CancellationTokenRegistration _appStoppingRegistration;
        private CancellationTokenRegistration _appStoppedRegistration;

        public AppLifetimeEventsHostedService(ILogger<AppLifetimeEventsHostedService> logger, IHostApplicationLifetime appLifetime, IHostEnvironment env)
        {
            _appLifetime = appLifetime;
            _env = env;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RegisterAppLifetimeHandlers();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void RegisterAppLifetimeHandlers()
        {
            _appStartedRegistration = _appLifetime.ApplicationStarted.Register(App_Started);
            _appStoppingRegistration = _appLifetime.ApplicationStopping.Register(App_Stopping);
            _appStoppedRegistration = _appLifetime.ApplicationStopped.Register(App_Stopped);
        }

        private void App_Started() => App_Event("Started");

        private void App_Stopping() => App_Event("Stopping");

        private void App_Stopped() => App_Event("Stopped");

        private void App_Event(string eventType)
        {
            var message = $"[{DateTime.UtcNow}] - [{nameof(IHostApplicationLifetime)}] - {eventType} on {_env.EnvironmentName}";
            _logger.LogInformation(message);
        }

        #region IDispose

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _appStartedRegistration.Dispose();
                _appStoppingRegistration.Dispose();
                _appStoppedRegistration.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDispose
    }
}