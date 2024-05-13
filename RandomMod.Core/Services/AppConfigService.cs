using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Services;

public class AppConfigService : ConfigBase, IConfig
{
    public string GameRootPath = string.Empty;
    public string OutputFolder = string.Empty;
    public override string GetFileName() => "UserConfig.json";
}