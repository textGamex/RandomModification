using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Game;
using RandomMod.Core.Services;
using RandomMod.Core.ViewModels;
using RandomMod.Core.Views;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RandomMod.Core;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json");
#if DEBUG
        builder.Environment.EnvironmentName = "Development";
        builder.Configuration.AddJsonFile("appsettings.Development.json");
#endif
        
        builder.Services.AddSingleton<App>();
        builder.Services.AddHostedService<HostedService<App, MainWindow>>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<INavigationWindow, MainNavigationUserControl>();

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<MainWindowViewModel>();
        builder.Services.AddSingleton<StateConfigPage>();
        builder.Services.AddSingleton<StateConfigViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<MainNavigationUserControl>();
        builder.Services.AddSingleton<MainNavigationViewModel>();
        builder.Services.AddSingleton<GameResourcesSelectorUserControl>();
        builder.Services.AddSingleton<GameResourcesSelectorViewModel>();

        builder.Services.AddSingleton<AppConfigService>(_ => AppConfigService.Load());
        builder.Services.AddSingleton<IPageService, PageService>();
        builder.Services.AddSingleton<IContentDialogService, ContentDialogService>();
        builder.Services.AddSingleton<DialogService>();
        builder.Services.AddSingleton<GameResourcesService>();

        if (Enum.TryParse<LogLevel>(builder.Configuration["Logging:LogLevel:Default"].AsSpan(), out var logLevel))
        {
            builder.Logging.SetMinimumLevel(logLevel);
        }

        var host = builder.Build();
        host.Run();
    }
}