using RandomMod.Core.Game;
using RandomMod.Core.Game.Parser;
using RandomMod.Core.Game.State;
using RandomMod.Core.Services;
using RandomMod.Core.Services.Game;

namespace RandomMod.Tests.Game.State;

[TestFixture]
[TestOf(typeof(RandomizeStateVisitor))]
public class RandomizeStateVisitorTests
{
    private const string Content =
        """
        state = {
            id = 1
            manpower = 1000
        }
        """;

    [Test]
    public void VisitorTest()
    {
        var node = new CwToolsParser("test.txt", Content).GetResult();
        var stateNode = node.Child(ScriptKeyWords.State).Value;
        var visitor = new RandomizeStateVisitor(new StateGenerator(new GameResourcesService(), new StateConfigService()));

        visitor.Visit(node);

        Assert.Multiple(() =>
        {
            var manpower = stateNode.Leafs(ScriptKeyWords.Manpower).ToArray();
            Assert.That(manpower[0].ValueText, Is.EqualTo(visitor.RandomManpower.ToString()));
            Assert.That(manpower, Has.Length.EqualTo(1));
        });
    }
}