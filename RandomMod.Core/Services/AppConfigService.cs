using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RandomMod.Core.Services;

public class AppConfigService
{
    public string GameRootPath { get; set; } = string.Empty;
    public string OutputFolder { get; set; } = string.Empty;

    public static readonly string FilePath = Path.Combine(App.ConfigFolder, "UserConfig.json");
    private static readonly ILogger<AppConfigService> Logger = App.Current.GetRequiredService<ILogger<AppConfigService>>();

    public static AppConfigService Load()
    {
        if (!File.Exists(FilePath))
        {
            return new AppConfigService();
        }
        var content = File.ReadAllText(FilePath);
        var config = JsonSerializer.Deserialize<AppConfigService>(content);
        if (config is null)
        {
            config = new AppConfigService();
            Logger.LogWarning("用户配置文件反序列化失败, 内容:{Content}", content);
        }
        Logger.LogInformation("用户配置文件加载成功");
        return config;
    }

    public void Save()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this));
    }
}