using CWTools.Process;
using HOI_Error_Tools.Logic.HOIParser;
using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Game.State;

public class RandomizeStateVisitor : INodeVisitor
{
    public void Visitor(Node rootNode)
    {
        if (rootNode.HasNot(ScriptKeyWords.State))
        {
            return;
        }

        var stateNode = rootNode.GetChild(ScriptKeyWords.State);
        RandomizeState(stateNode);
    }

    private void RandomizeState(Node stateNode)
    {
    }
}