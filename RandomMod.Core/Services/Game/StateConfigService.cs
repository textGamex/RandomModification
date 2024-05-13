using System.Text.Json;
using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Services.Game;

public class StateConfigService : ConfigBase, IConfig
{
    public int ManpowerMinRandom = 1000;
    public int ManpowerMaxRandom = 15000;
    public override string GetFileName() => "GameStateConfig.json";
}