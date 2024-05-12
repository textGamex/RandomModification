using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _gameRootPath;

    [ObservableProperty]
    private string _outputFolderPath;

    private readonly AppConfigService _configService;

    public SettingsViewModel(AppConfigService configService)
    {
        _configService = configService;

        _gameRootPath = _configService.GameRootPath;
        _outputFolderPath = _configService.OutputFolder;
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

    [RelayCommand]
    private void SelectOutputFolder()
    {
        var dialog = new OpenFolderDialog { Title = "选择输出文件夹" };
        if (dialog.ShowDialog() == true)
        {
            OutputFolderPath = dialog.FolderName;
        }
    }

    partial void OnGameRootPathChanged(string value)
    {
        _configService.GameRootPath = value;
    }

    partial void OnOutputFolderPathChanged(string value)
    {
        _configService.OutputFolder = value;
    }
}