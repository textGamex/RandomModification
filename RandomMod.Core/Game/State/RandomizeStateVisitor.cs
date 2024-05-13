using CWTools.Parser;
using CWTools.Process;
using CWTools.Utilities;
using RandomMod.Core.Extensions;
using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Game.State;

public class RandomizeStateVisitor : INodeVisitor
{
    public int RandomManpower { get; private set; }
    private readonly StateGenerator _stateGenerator;

    public RandomizeStateVisitor(StateGenerator stateGenerator)
    {
        _stateGenerator = stateGenerator;
        RandomManpower = 0;
    }

    public void Visit(Node rootNode)
    {
        if (!rootNode.TryGetChild(ScriptKeyWords.State, out var node))
        {
            return;
        }

        RandomizeState(node);
    }

    private void RandomizeState(Node stateNode)
    {
        var ownerTag = GetOwnerTag(stateNode);
        RandomManpower = _stateGenerator.GetManpowerByCountry(ownerTag ?? string.Empty);
        ReplaceManpower(stateNode, RandomManpower);
    }

    private static string? GetOwnerTag(Node node)
    {
        return node.TryGetChild(ScriptKeyWords.History, out node)
            ? node.Leafs(ScriptKeyWords.Owner).FirstOrDefault()?.ValueText
            : null;
    }

    private static void ReplaceManpower(Node stateNode, int manpower)
    {
        for (var i = 0; i < stateNode.AllArray.Length; i++)
        {
            var child = stateNode.AllArray[i];
            if (child.IsLeafC && child.leaf.Key == ScriptKeyWords.Manpower)
            {
                stateNode.AllArray[i] =
                    Leaf.Create(
                        Types.KeyValueItem.NewKeyValueItem(Types.Key.NewKey(ScriptKeyWords.Manpower),
                            Types.Value.NewInt(manpower), Types.Operator.Equals), Position.range.Zero);
            }
        }
    }
}