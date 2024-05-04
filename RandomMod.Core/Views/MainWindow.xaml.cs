using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;
using RandomMod.Core.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RandomMod.Core.Views;

public partial class MainWindow
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly AppConfigService _configService;
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(IHostApplicationLifetime hostApplicationLifetime, MainWindowViewModel model, 
        AppConfigService configService, IServiceProvider serviceProvider)
    {
        DataContext = model;
        _hostApplicationLifetime = hostApplicationLifetime;
        _configService = configService;
        _serviceProvider = serviceProvider;
        InitializeComponent();

        if (string.IsNullOrWhiteSpace(configService.GameRootPath))
        {
            ContentControl.Content = serviceProvider.GetRequiredService<GameResourcesSelectorUserControl>();
        }
        else
        {
            ContentControl.Content = serviceProvider.GetRequiredService<MainNavigationUserControl>();
        }
        WeakReferenceMessenger.Default.Register<FinishAppFirstConfigMessage>(this,
            (_,_) => ContentControl.Content = _serviceProvider.GetRequiredService<MainNavigationUserControl>());
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _configService.Save();
        _hostApplicationLifetime.StopApplication();
    }


}