using System.Windows.Controls;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel model)
    {
        InitializeComponent();

        DataContext = model;
    }
}