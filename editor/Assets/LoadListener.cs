using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using UnityEngine;

public class LoadListener : GravisBaseListener
{
    private readonly Dictionary<string, Node> definitions = new Dictionary<string, Node>();
    private readonly Stack<Subspace> subspaceStack = new Stack<Subspace>();
    public readonly List<Node> nodes = new List<Node>();
    public readonly List<Subspace> subspaces = new List<Subspace>();

    private Node CreateNode(GravisParser.Node_defContext context) {
        var node = new Node();
        if (subspaceStack.Count != 0)
        {
            node.subspace = subspaceStack.Peek();
        }
        if (context.input_def() != null)
        {
            if (subspaceStack.Count != 0)
            {
                var subspace = subspaceStack.Peek();
                subspace.input = node;
            }
            node.type = NodeType.Input;
            node.text = "I";
        }
        else if (context.output_def() != null)
        {
            if (subspaceStack.Count != 0)
            {
                var subspace = subspaceStack.Peek();
                subspace.output = node;
            }
            node.type = NodeType.Output;
            node.text = "O";
        }
        else if (context.const_def() != null)
        {
            node.type = NodeType.Constant;
            node.text = context.const_def().NUMBER().GetText();
        }
        else if (context.if_def() != null)
        {
            node.type = NodeType.If;
            node.text = context.if_def().comp_op().GetText();
        }
        else if (context.opr_def() != null)
        {
            node.type = NodeType.Operator;
            node.text = context.opr_def().arith_op().GetText();
        }
        else if (context.subspace_def() != null)
        {
            node.type = NodeType.Subspace;
            node.text = "S";
            var subspace = new Subspace(node);
            subspaces.Add(subspace);
            subspaceStack.Push(subspace);
        }
        else if (context.self_subspace_def() != null)
        {
            node.type = NodeType.SelfSubspace;
            node.text = "SS";
        }
        else
            node.text = "?";
        nodes.Add(node);
        return node;
    }

    public override void EnterDef_stmt([NotNull] GravisParser.Def_stmtContext context)
    {
        var nodeName = context.NAME().GetText();
        var node = CreateNode(context.node_def());
        definitions[nodeName] = node;
    }

    private Node getDottedName(GravisParser.Dotted_nameContext context) {
        var nodeInst = context.node_inst();
        Node node;
        if (nodeInst.NAME() != null)
        {
            var nodeName = nodeInst.NAME().GetText();
            node = definitions[nodeName];
        }
        else
        {
            node = CreateNode(nodeInst.node_def());
        }
        return node;
    }

    private List<Node> getOutputs(GravisParser.Dotted_nameContext context, Node node)
    {
        if (context.NAME() != null && context.NAME().GetText() == "false")
        {
            return node.falseOutputs;
        }
        return node.trueOutputs;
    }

    public override void EnterLink_stmt([NotNull] GravisParser.Link_stmtContext context)
    {
        var pair = context.dotted_name();
        var left = getDottedName(pair[0]);
        var right = getDottedName(pair[1]);
        getOutputs(pair[0], left).Add(right);
        right.inputs.Add(left);
    }

    public override void ExitSubspace_def([NotNull] GravisParser.Subspace_defContext context)
    {
        subspaceStack.Pop();
    }
}
