using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class NodeExtensions
    {
        public static void AggregateAllRelationships(this Node node, Action<Node, List<Node>> action)
        {
            var trueOutputs = new List<Node>(node.trueOutputs);
            trueOutputs.ForEach(value => action.Invoke(value, node.trueOutputs));
            
            var falseOutputs = new List<Node>(node.falseOutputs);
            falseOutputs.ForEach(value => action.Invoke(value, node.falseOutputs));
            
            var inputs = new List<Node>(node.inputs);
            inputs.ForEach(value => action.Invoke(value, node.inputs));
        }

        public static void ClearAllRelationships(this Node node, List<Node> graph)
        {
            node.trueOutputs.Clear();
            node.falseOutputs.Clear();
            node.inputs.Clear();

            foreach (var item in graph)
            {
                item.trueOutputs.Remove(node);
                item.falseOutputs.Remove(node);
                item.inputs.Remove(node);
            }
        }

        public static List<Node> ConcatAllRelationships(this Node node)
        {
            var result = node
                .trueOutputs
                .Concat(node.falseOutputs)
                .Concat(node.inputs)
                .ToList();

            return result;
        }
    }
}