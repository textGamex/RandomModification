using RandomMod.Core.Game.State;
using RandomMod.Core.Services;
using RandomMod.Core.Services.Game;

namespace RandomMod.Tests.Game.State;

[TestFixture]
[TestOf(typeof(StateGenerator))]
public class StateGeneratorTests
{
    [Test]
    public void GetManpowerByCountryTest_CountryIsExist()
    {
        var stateGenerator =
            new StateGenerator(
                new GameResourcesService { CountryStateCount = new Dictionary<string, int>() { { "AAA", 3 } } },
                new StateConfigService());

        Assert.DoesNotThrow(() =>
        {
            stateGenerator.GetManpowerByCountry("AAA");
        });
    }

    [Test]
    public void GetManpowerByCountryTest_CountryIsNotExist()
    {
        var stateGenerator =
            new StateGenerator(new GameResourcesService(), new StateConfigService());

        Assert.DoesNotThrow(() =>
        {
            stateGenerator.GetManpowerByCountry("TAT");
            stateGenerator.GetManpowerByCountry("T");
            stateGenerator.GetManpowerByCountry(string.Empty);
        });
    }
}