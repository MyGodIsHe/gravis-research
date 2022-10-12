using System.Collections.Generic;
using UI.Selection.Strategies;

namespace UI.Selection
{
    public static class SelectionHelper
    {
        private static readonly IDictionary<NodeType, string> _texts = new Dictionary<NodeType, string>()
        {
            {NodeType.Input, "I"},
            {NodeType.Output, "O"},
            {NodeType.Subspace, "SS"},
            {NodeType.SelfSubspace, "SS"}
        };

        public static NodeTextStrategyBase GetStrategy(NodeType type)
        {
            switch (type)
            {
                case NodeType.Constant:
                case NodeType.If:
                case NodeType.Operator:
                    return new TypingNodeTextStrategy();
                case NodeType.Input:
                case NodeType.Output:
                case NodeType.Subspace:
                case NodeType.SelfSubspace:
                default:
                    var text = _texts[type];
                    return new FixedNodeTextStrategy(text);
            }
        }
    }
}