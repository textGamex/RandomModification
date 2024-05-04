using System.Windows;
using Microsoft.Extensions.Hosting;

namespace RandomMod.Core.Services;

public class HostedService<TApplication, TMainWindow> : IHostedService
    where TApplication : Application
    where TMainWindow : Window
{
    private readonly TApplication _application;
    private readonly TMainWindow _mainWindow;

    public HostedService(TApplication application, TMainWindow mainWindow, IHostApplicationLifetime hostApplicationLifetime)
    {
        _application = application;
        _mainWindow = mainWindow;

        hostApplicationLifetime.ApplicationStopping.Register(application.Shutdown);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        _application.Run(_mainWindow);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}