using RandomMod.Core.Game.Parser;
using RandomMod.Core.Services;

namespace RandomMod.Core.Game.State.Tests;

[TestFixture()]
public class RandomizeStateVisitorTests
{
    private const string Content = 
        """
        state = {
            id = 1
            manpower = 1000
        }
        """;

    [Test()]
    public void VisitorTest()
    {
        var node = new CwToolsParser("test.txt", Content).GetResult();
        var stateNode = node.Child(ScriptKeyWords.State).Value;
        var visitor = new RandomizeStateVisitor(new GameResourcesService());

        visitor.Visit(node);

        Assert.Multiple(() =>
        {
            Assert.That(stateNode.Leafs(ScriptKeyWords.Manpower).First().ValueText, Is.EqualTo(visitor.RandomManpower.ToString()));
            Assert.That(stateNode.Leafs(ScriptKeyWords.Manpower).Count(), Is.EqualTo(1));
        });
    }
}