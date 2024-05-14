using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using Wpf.Ui.Controls;

namespace RandomMod.Core.Behaviors;

public partial class InputOnlyNumberBehavior : Behavior<TextBox>
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberOnlyRegex();

    protected override void OnAttached()
    {
        AssociatedObject.PreviewTextInput += NumberOnly_OnPreviewTextInput;
        InputMethod.SetIsInputMethodEnabled(AssociatedObject, false);
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewTextInput -= NumberOnly_OnPreviewTextInput;
    }

    private static void NumberOnly_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!NumberOnlyRegex().IsMatch(e.Text))
        {
            e.Handled = true;
        }
    }
}