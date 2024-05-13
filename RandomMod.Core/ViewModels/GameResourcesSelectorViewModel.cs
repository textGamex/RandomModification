using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using RandomMod.Core.Interfaces;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class GameResourcesSelectorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _gameRootPath = string.Empty;
    [ObservableProperty]
    private string _outputFolder = string.Empty;

    private readonly AppConfigService _appConfig;
    private readonly IConfigManagerService _configManagerService;

    public GameResourcesSelectorViewModel(AppConfigService appConfig, IConfigManagerService configManagerService)
    {
        _appConfig = appConfig;
        _configManagerService = configManagerService;
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
    private void SelectOutputFolderPath()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "请选择输出文件夹"
        };

        if (dialog.ShowDialog() == true)
        {
            OutputFolder = dialog.FolderName;
        }
    }

    [RelayCommand]
    private void Finish()
    {
        if (string.IsNullOrEmpty(GameRootPath) || string.IsNullOrEmpty(OutputFolder))
        {
            return;
        }
        _appConfig.GameRootPath = GameRootPath;
        _appConfig.OutputFolder = OutputFolder;

        Task.Run(() => _configManagerService.SaveConfig(_appConfig));
        WeakReferenceMessenger.Default.Send(new FinishAppFirstConfigMessage());
    }
}