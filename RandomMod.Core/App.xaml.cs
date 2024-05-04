using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace RandomMod.Core;

public partial class App : Application
{
    public static readonly string ConfigFolder = Path.Combine(Environment.CurrentDirectory, "Config");
    public new static App Current => (App)Application.Current;
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    public T GetRequiredService<T>()
    {
        return (T)_serviceProvider.GetRequiredService(typeof(T));
    }
}