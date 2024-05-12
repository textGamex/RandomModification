using System.Windows.Controls;
using RandomMod.Core.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RandomMod.Core.Views;

public partial class MainNavigationUserControl : UserControl, INavigationWindow
{
    public MainNavigationUserControl(MainNavigationViewModel model, INavigationService navigationService)
    {
        InitializeComponent();

        DataContext = model;
        navigationService.SetNavigationControl(RootNavigation);
        Loaded += (_, _) => RootNavigation.Navigate("default");
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
        throw new NotImplementedException();
    }

    public void CloseWindow()
    {
        throw new NotImplementedException();
    }

    // ?
    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    #endregion
}