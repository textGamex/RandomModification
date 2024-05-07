using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Game;
using RandomMod.Core.Game.Parser;
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

    public MainWindowViewModel(AppConfigService configService, ILogger<MainWindowViewModel> logger, GameResourcesService gameResourcesService)
    {
        _configService = configService;
        _logger = logger;
        _gameResourcesService = gameResourcesService;
    }

    public async Task StartParse()
    {
        var timestamp = Stopwatch.GetTimestamp();
        var paths = new GameResourcesPath(_configService.GameRootPath);
        var sum = paths.StatesFilePathList.Count;
        var count = 0;
        //var tasks = new List<Task<CwToolsParser>>(sum);
        var tasks = new List<CwToolsParser>(sum);

        foreach (var path in paths.StatesFilePathList)
        {
            await Task.Run(async () =>
            {
                _logger.LogDebug("开始解析: {Path}", path);
                tasks.Add(await Task.Run(() => new CwToolsParser(path)));
                Interlocked.Increment(ref count);
                App.Current.Dispatcher.Invoke(() =>
                {
                    ParseProgress = ((double)count / sum) * 100;
                });
            });
        }
        //foreach (var path in list)
        //{
        //    tasks.Add(CwToolsParser.Parser(path));
        //}
        //await Task.WhenAll(tasks);
        var elapsedTime = Stopwatch.GetElapsedTime(timestamp);
        ParseText = "解析完成";
        _gameResourcesService.States = tasks;
        _logger.LogInformation("解析完成, 耗时: {Time:F2} s", elapsedTime.TotalSeconds);
    }
}