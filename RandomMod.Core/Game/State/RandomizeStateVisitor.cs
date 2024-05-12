using System.ComponentModel.Design;
using CWTools.Parser;
using CWTools.Process;
using CWTools.Utilities;
using MathNet.Numerics.Random;
using RandomMod.Core.Extensions;
using RandomMod.Core.Interfaces;
using RandomMod.Core.Services;

namespace RandomMod.Core.Game.State;

public class RandomizeStateVisitor : INodeVisitor
{
    

    public int RandomManpower { get; private set; }

    private static readonly MersenneTwister Random = new(true);
    private readonly GameResourcesService _resourcesService;

    public RandomizeStateVisitor(GameResourcesService resourcesService)
    {
        _resourcesService = resourcesService;
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
        var ownerStateAmount = _resourcesService.CountryStateCount.GetValueOrDefault(ownerTag ?? string.Empty, 1);
        RandomManpower = GetManpower(ownerStateAmount);
        ReplaceManpower(stateNode, RandomManpower);
    }

    private static string? GetOwnerTag(Node node)
    {
        return node.TryGetChild(ScriptKeyWords.History, out node)
            ? node.Leafs(ScriptKeyWords.Owner).FirstOrDefault()?.ValueText
            : null;
    }

    private static int GetManpower(int stateAmount)
    {
        var random = Random.Next(100, 15000);
        return (int)(Math.Log10(stateAmount) * 10000 + random);
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