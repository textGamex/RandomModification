using System.Windows.Controls;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class MainConfigPage : Page
{
    public MainConfigPage(MainConfigViewModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}