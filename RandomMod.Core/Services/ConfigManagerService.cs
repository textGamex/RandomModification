using System.IO;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Services;

public sealed class ConfigManagerService : IConfigManagerService
{
    private readonly ILogger<ConfigManagerService> _logger;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true, IncludeFields = true};
    private readonly List<(IConfig, Type)> _configs = new(4);

    public ConfigManagerService(ILogger<ConfigManagerService> logger)
    {
        _logger = logger;
    }

    public void SaveAll()
    {
        foreach (var (config, type) in _configs)
        {
            if (config.IsChanged())
            {
                SaveConfig(config, type);
            }
        }
    }

    private void SaveConfig(IConfig config, Type type)
    {
        File.WriteAllText(Path.Combine(App.ConfigFolder, config.GetFileName()), JsonSerializer.Serialize(config, type, _options),
            Encoding.UTF8);
        _logger.LogInformation("保存配置文件 '{FileName}'", ((IConfig)config).GetFileName());
    }

    public void SaveConfig<T>(T config) where T : IConfig
    {
        File.WriteAllText(Path.Combine(App.ConfigFolder, config.GetFileName()), JsonSerializer.Serialize(config, _options),
            Encoding.UTF8);
        _logger.LogInformation("保存配置文件 '{FileName}'", config.GetFileName());
    }

    public T LoadConfig<T>() where T : IConfig, new()
    {
        var config = new T();
        _configs.Add((config, typeof(T)));
        var filePath = Path.Combine(App.ConfigFolder, config.GetFileName());
        if (!File.Exists(filePath))
        {
            _logger.LogInformation("初始化配置文件 '{FileName}'", config.GetFileName());
            return config;
        }

        var content = File.ReadAllText(filePath, Encoding.UTF8);
        var result = JsonSerializer.Deserialize<T>(content, _options);
        if (result is null)
        {
            _logger.LogWarning("反序列化 '{FileName}' 失败, 内容: {Content}", config.GetFileName(), result);
            return config;
        }

        _logger.LogInformation("加载配置文件 '{FileName}'", config.GetFileName());
        _configs.RemoveAt(_configs.Count - 1);
        _configs.Add((result, typeof(T)));

        return result;
    }
}