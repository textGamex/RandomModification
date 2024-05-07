using CWTools.Process;

namespace RandomMod.Core.Interfaces;

public interface INodeVisitor
{
    void Visitor(Node rootNode);
}