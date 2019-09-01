using Antlr4.Runtime.Misc;
using System.Collections.Generic;

public class LoadListener : GravisBaseListener
{
    private readonly Dictionary<string, Node> definitions = new Dictionary<string, Node>();
    public readonly List<Node> nodes = new List<Node>();

    private Node CreateNode(GravisParser.Node_defContext context) {
        var node = new Node();
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

    public override void EnterLink_stmt([NotNull] GravisParser.Link_stmtContext context)
    {
        var pair = context.dotted_name();
        var left = getDottedName(pair[0]);
        var right = getDottedName(pair[1]);
        left.outputs.Add(right);
        right.inputs.Add(left);
    }
}
