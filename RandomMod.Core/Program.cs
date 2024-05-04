using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomMod.Core.Services;
using RandomMod.Core.ViewModels;
using RandomMod.Core.Views;
using Wpf.Ui;

namespace RandomMod.Core;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<App>();
        builder.Services.AddHostedService<HostedService<App, MainWindow>>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<INavigationWindow, MainWindow>();

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<MainWindowViewModel>();
        builder.Services.AddTransient<GameResourcesSelectPage>();
        builder.Services.AddTransient<GameResourcesSelectViewModel>();
        builder.Services.AddSingleton<StateConfigPage>();
        builder.Services.AddSingleton<StateConfigViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsViewModel>();

        builder.Services.AddSingleton<UserConfigService>(_ => UserConfigService.Load());
        builder.Services.AddSingleton<IPageService, PageService>();

        var host = builder.Build();
        host.Run();
    }
}