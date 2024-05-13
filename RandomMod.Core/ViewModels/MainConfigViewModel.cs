using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CWTools.CSharp;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Game.State;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;
using RandomMod.Core.Services.Game;

namespace RandomMod.Core.ViewModels;

public partial class MainConfigViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isParseComplete;
    private readonly GameResourcesService _gameResourcesService;
    private readonly ILogger<MainConfigViewModel> _logger;
    private readonly AppConfigService _configService;
    private readonly StateConfigService _stateConfigService;

    public MainConfigViewModel(GameResourcesService gameResourcesService, ILogger<MainConfigViewModel> logger,
        AppConfigService configService, StateConfigService stateConfigService)
    {
        _gameResourcesService = gameResourcesService;
        _logger = logger;
        _configService = configService;
        _stateConfigService = stateConfigService;
        WeakReferenceMessenger.Default.Register<ParseFilesCompleteMessage>(
            this, (_, _) => IsParseComplete = true);
    }

    [RelayCommand]
    private async Task StartAsync()
    {
        var visitor = new RandomizeStateVisitor(new StateGenerator(_gameResourcesService, _stateConfigService));
        foreach (var state in _gameResourcesService.States)
        {
            visitor.Visit(state);
            await File.WriteAllTextAsync(
                Path.Combine(_configService.OutputFolder, Path.GetFileName(state.Position.FileName)),
                GetAnticipant(state.ToRaw.PrettyPrint())).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 去除多余的文件名节点
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetAnticipant(string value)
    {
        var startIndex = value.IndexOf('{');
        var endIndex = value.LastIndexOf('}');
        return value[(startIndex + 1)..(endIndex - 1)];
    }
}