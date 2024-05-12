using System.Collections.Concurrent;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CWTools.Process;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Extensions;
using RandomMod.Core.Game;
using RandomMod.Core.Game.Parser;
using RandomMod.Core.Messages;
using RandomMod.Core.Services;

namespace RandomMod.Core.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private double _parseProgress;
    [ObservableProperty]
    private string _parseText = "正在解析";

    private readonly AppConfigService _configService;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly GameResourcesService _gameResourcesService;

    public MainWindowViewModel(AppConfigService configService, ILogger<MainWindowViewModel> logger,
        GameResourcesService gameResourcesService)
    {
        _configService = configService;
        _logger = logger;
        _gameResourcesService = gameResourcesService;
    }

    public async Task StartParseAsync()
    {
        var timestamp = Stopwatch.GetTimestamp();
        var paths = new GameResourcesPath(_configService.GameRootPath);
        var sum = paths.StatesFilePathList.Count;
        var count = 0;

        var nodes = new List<Node>(sum);
        var countries = new Dictionary<string, int>();
        
        _logger.LogInformation("开始解析, 文件数量: {Count}", paths.StatesFilePathList.Count);
        foreach (var path in paths.StatesFilePathList)
        {
            var parser = await Task.Run(() => new CwToolsParser(path));
            count++;
            var count1 = count;
            App.Current.Dispatcher.Invoke(() => { ParseProgress = (double)count1 / sum * 100; });

            if (parser.IsFailure)
            {
                _logger.LogWarning("解析失败: {Path}", path);
                return;
            }
            var node = parser.GetResult();
            nodes.Add(node);
            if (!node.TryGetChild(ScriptKeyWords.State, out node))
            {
                return;
            }
            if (!node.TryGetChild(ScriptKeyWords.History, out node))
            {
                return;
            }
            var country = node.Leafs(ScriptKeyWords.Owner).FirstOrDefault();
            if (country is null)
            {
                _logger.LogWarning("未找到 owner: {Path}", path);
                return;
            }

            if (countries.TryGetValue(country.ValueText, out var value))
            {
                countries[country.ValueText] = value + 1;
            }
            else
            {
                countries[country.ValueText] = 1;
            }
        }

        var elapsedTime = Stopwatch.GetElapsedTime(timestamp);

        _gameResourcesService.States = nodes;
        _gameResourcesService.CountryStateCount = countries;
        ParseText = "解析完成";
        WeakReferenceMessenger.Default.Send(new ParseFilesCompleteMessage());
        _logger.LogInformation("解析完成, 耗时: {Time:F2} s", elapsedTime.TotalSeconds);
    }
}