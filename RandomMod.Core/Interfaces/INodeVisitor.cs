using CWTools.Process;

namespace RandomMod.Core.Interfaces;

public interface INodeVisitor
{
    void Visit(Node rootNode);
}