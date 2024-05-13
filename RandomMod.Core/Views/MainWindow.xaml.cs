using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomMod.Core.Interfaces;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class MainWindow
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfigManagerService _configManagerService;

    public MainWindow(IHostApplicationLifetime hostApplicationLifetime, MainWindowViewModel model, 
        AppConfigService appConfigService, IServiceProvider serviceProvider, IConfigManagerService configManagerService)
    {
        DataContext = model;
        _hostApplicationLifetime = hostApplicationLifetime;
        _configManagerService = configManagerService;
        InitializeComponent();

        if (string.IsNullOrWhiteSpace(appConfigService.GameRootPath) || string.IsNullOrWhiteSpace(appConfigService.OutputFolder))
        {
            ContentControl.Content = serviceProvider.GetRequiredService<GameResourcesSelectorUserControl>();
        }
        else
        {
            ContentControl.Content = serviceProvider.GetRequiredService<MainNavigationUserControl>();
            Task.Run(model.StartParseAsync);
        }
        WeakReferenceMessenger.Default.Register<FinishAppFirstConfigMessage>(this,
            (_, _) =>
            {
                ContentControl.Content = serviceProvider.GetRequiredService<MainNavigationUserControl>();
                model.StartParseAsync().ConfigureAwait(false);
            });
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _configManagerService.SaveAll();
        _hostApplicationLifetime.StopApplication();
    }
}