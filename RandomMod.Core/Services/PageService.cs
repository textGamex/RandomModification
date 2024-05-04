using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace RandomMod.Core.Services;

public class PageService : IPageService
{
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T? GetPage<T>() where T : class
    {
        if (!typeof(FrameworkElement).IsSubclassOf(typeof(T)))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return (T?)_serviceProvider.GetRequiredService(typeof(T));
    }

    public FrameworkElement? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return _serviceProvider.GetService(pageType) as FrameworkElement;
    }
}