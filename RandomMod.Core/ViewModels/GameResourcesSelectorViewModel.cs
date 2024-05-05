using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class GameResourcesSelectorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _gameRootPath = string.Empty;

    private readonly AppConfigService _configService;

    public GameResourcesSelectorViewModel(AppConfigService configService)
    {
        _configService = configService;
    }

    [RelayCommand]
    private void SelectGameRootPath()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "请选择游戏根目录"
        };

        if (dialog.ShowDialog() == true)
        {
            GameRootPath = dialog.FolderName;
        }
    }

    [RelayCommand]
    private void Finish()
    {
        if (string.IsNullOrWhiteSpace(GameRootPath))
        {
            return;
        }
        _configService.GameRootPath = GameRootPath;
        Task.Run(_configService.Save);
        WeakReferenceMessenger.Default.Send(new FinishAppFirstConfigMessage());
    }
}