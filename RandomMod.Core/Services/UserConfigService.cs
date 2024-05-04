using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RandomMod.Core.Services;

public class UserConfigService
{
    public string GameRootPath { get; set; } = string.Empty;

    public static readonly string FilePath = Path.Combine(App.ConfigFolder, "UserConfig.json");
    private static readonly ILogger<UserConfigService> _logger = App.Current.GetRequiredService<ILogger<UserConfigService>>();

    public static UserConfigService Load()
    {
        if (!File.Exists(FilePath))
        {
            return new UserConfigService();
        }
        var content = File.ReadAllText(FilePath);
        var config = JsonSerializer.Deserialize<UserConfigService>(content);
        if (config is null)
        {
            config = new UserConfigService();
            _logger.LogWarning("用户配置文件反序列化失败, 内容:{Content}", content);
        }
        return config;
    }

    public void Save()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this));
    }
}