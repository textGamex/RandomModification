using System.Windows;
using Microsoft.Extensions.Hosting;
using RandomMod.Core.Services;
using RandomMod.Core.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RandomMod.Core.Views;

public partial class MainWindow : INavigationWindow
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly UserConfigService _configService;

    public MainWindow(IHostApplicationLifetime hostApplicationLifetime, INavigationService navigationService,
        MainWindowViewModel model, UserConfigService configService)
    {
        DataContext = model;
        _hostApplicationLifetime = hostApplicationLifetime;
        _configService = configService;
        InitializeComponent();

        navigationService.SetNavigationControl(RootNavigation);
        //RootNavigation.SetServiceProvider(serviceProvider);
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_configService.GameRootPath))
        {
            RootNavigation.Navigate(typeof(SettingsPage));
        }
        else
        {
            RootNavigation.Navigate(typeof(StateConfigPage));
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _configService.Save();
        _hostApplicationLifetime.StopApplication();
    }

    #region 导航实现

    public INavigationView GetNavigation()
    {
        return RootNavigation;
    }

    public bool Navigate(Type pageType)
    {
        return RootNavigation.Navigate(pageType);
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        RootNavigation.SetServiceProvider(serviceProvider);
    }

    public void SetPageService(IPageService pageService)
    {
        RootNavigation.SetPageService(pageService);
    }

    public void ShowWindow()
    {
        Show();
    }

    public void CloseWindow()
    {
        Close();
    }

    // ?
    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    #endregion
}