using System;
using System.Collections.Generic;

namespace Utils
{
    public static class NodeExtensions
    {
        public static void AggregateAllRelationships(this Node node, Action<Node> action)
        {
            var trueOutputs = new List<Node>(node.trueOutputs);
            trueOutputs.ForEach(action.Invoke);
            
            var falseOutputs = new List<Node>(node.falseOutputs);
            falseOutputs.ForEach(action.Invoke);
            
            var inputs = new List<Node>(node.inputs);
            inputs.ForEach(action.Invoke);
        }
        
        public static void AggregateAllRelationships(this Node node, Action<Node, List<Node>> action)
        {
            var trueOutputs = new List<Node>(node.trueOutputs);
            trueOutputs.ForEach(value => action.Invoke(value, node.trueOutputs));
            
            var falseOutputs = new List<Node>(node.falseOutputs);
            falseOutputs.ForEach(value => action.Invoke(value, node.falseOutputs));
            
            var inputs = new List<Node>(node.inputs);
            inputs.ForEach(value => action.Invoke(value, node.inputs));
        }
    }
}