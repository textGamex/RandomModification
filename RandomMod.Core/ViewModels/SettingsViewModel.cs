using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _gameRootPath;

    private readonly UserConfigService _configService;

    public SettingsViewModel(UserConfigService configService)
    {
        _configService = configService;

        _gameRootPath = _configService.GameRootPath;
    }

    [RelayCommand]
    private void SelectGameRootPath()
    {
        var dialog = new OpenFolderDialog{Title = "选择HOI4根目录"};
        if (dialog.ShowDialog() == true)
        {
            GameRootPath = dialog.FolderName;
        }
    }

    partial void OnGameRootPathChanged(string value)
    {
        _configService.GameRootPath = value;
    }
}