using Nodes.Enums;
using UI.Selection.Strategies;

namespace UI.Selection
{
    public static class ForceHelper
    {
        public static NodeForceStrategyBase GetStrategy(NodeType type)
        {
            switch (type)
            {
                case NodeType.Input:
                    var @out = new FixedNodeForceStrategy(ENodeForce.Out);
                    return @out;
                case NodeType.Output:
                case NodeType.Constant:
                    var @in = new FixedNodeForceStrategy(ENodeForce.In);
                    return @in;
                default:
                    var select = new SelectNodeForceStrategy();
                    return select;
            }
        }
    }
}