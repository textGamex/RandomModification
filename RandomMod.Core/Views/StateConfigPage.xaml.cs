using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using RandomMod.Core.ViewModels;

namespace RandomMod.Core.Views;

public partial class StateConfigPage : Page
{
    public StateConfigPage(StateConfigViewModel model)
    {
        InitializeComponent();

        DataContext = model;
    }
}