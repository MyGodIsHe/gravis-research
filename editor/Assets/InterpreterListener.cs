using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using UnityEngine;

public class InterpreterListener : GravisBaseListener
{
    private readonly Fib fib;
    private readonly Dictionary<string, GameObject> definitions = new Dictionary<string, GameObject>();

    public InterpreterListener(Fib fib) {
        this.fib = fib;
    }

    private GameObject createNode(GravisParser.Node_defContext context) {
        var x = Random.Range(-5.0f, 5.0f);
        var y = Random.Range(-5.0f, 5.0f);
        var node = fib.Define(fib.cubeNode, new Vector3(x, y));
        return node;
    }

    public override void EnterDef_stmt([NotNull] GravisParser.Def_stmtContext context)
    {
        var nodeName = context.NAME().GetText();
        var node = createNode(context.node_def());
        definitions[nodeName] = node;
    }

    private GameObject getDottedName(GravisParser.Dotted_nameContext context) {
        var nodeInst = context.node_inst();
        GameObject node;
        if (nodeInst.NAME() != null)
        {
            var nodeName = nodeInst.NAME().GetText();
            node = definitions[nodeName];
        }
        else
        {
            node = createNode(nodeInst.node_def());
        }
        return node;
    }

    public override void EnterLink_stmt([NotNull] GravisParser.Link_stmtContext context)
    {
        var pair = context.dotted_name();
        var left = pair[0];
        var right = pair[1];
        fib.LineTo(getDottedName(left), getDottedName(right));
    }
}