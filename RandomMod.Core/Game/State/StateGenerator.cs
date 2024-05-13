using RandomMod.Core.Services;
using RandomMod.Core.Services.Game;

namespace RandomMod.Core.Game.State;

public sealed partial class StateGenerator
{
    private readonly ManpowerGenerator _manpowerGenerator;

    public StateGenerator(GameResourcesService resourcesService, StateConfigService stateConfigService)
    {
        _manpowerGenerator = new ManpowerGenerator(resourcesService, stateConfigService);
    }

    public int GetManpowerByCountry(string countryTag)
    {
        return _manpowerGenerator.GetManpowerByCountry(countryTag);
    }
}