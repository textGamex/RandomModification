using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Controls;

namespace RandomMod.Core.ViewModels;

public partial class MainNavigationViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];
    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    public MainNavigationViewModel()
    {
        NavigationItems =
        [
            new NavigationViewItem
            {
                Content = "地块",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Map24 },
                TargetPageType = typeof(Views.StateConfigPage)
            },
        ];

        NavigationFooter =
        [
            new NavigationViewItem
            {
                Content = "设置",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.SettingsPage)
            },
        ];
    }
}