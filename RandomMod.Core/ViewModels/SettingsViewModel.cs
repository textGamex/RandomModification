using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public string GameRootPath
    {
        get => _configService.GameRootPath;
        set => SetProperty(ref _configService.GameRootPath, value);
    }

    public string OutputFolderPath
    {
        get => _configService.OutputFolder;
        set => SetProperty(ref _configService.OutputFolder, value);
    }

    private readonly AppConfigService _configService;

    public SettingsViewModel(AppConfigService configService)
    {
        _configService = configService;
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
}