using System.Windows.Controls;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class GameResourcesSelectorUserControl : UserControl
{
    public GameResourcesSelectorUserControl(GameResourcesSelectorViewModel model)
    {
        DataContext = model;
        InitializeComponent();
    }

}