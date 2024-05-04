using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace RandomMod.Core.Services;

public class DialogService
{
    private readonly IContentDialogService _contentDialogService;

    public DialogService(IContentDialogService contentDialogService)
    {
        _contentDialogService = contentDialogService;
    }

    public async Task<MessageBoxResult> ShowMessageDialogAsync(string title, string message)
    {
        var dialog = new SimpleContentDialogCreateOptions()
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };

        var result = await _contentDialogService.ShowSimpleDialogAsync(dialog);
        switch (result)
        {
            case ContentDialogResult.Primary: return MessageBoxResult.Yes;
            case ContentDialogResult.Secondary: return MessageBoxResult.No;
            case ContentDialogResult.None: return MessageBoxResult.Cancel;
            default: return MessageBoxResult.None;
        }
    }

}