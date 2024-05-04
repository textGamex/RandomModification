using System.Windows.Controls;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class GameResourcesSelectPage : Page
{
    public GameResourcesSelectPage(GameResourcesSelectViewModel model)
    {
        InitializeComponent();

        DataContext = model;
    }
}