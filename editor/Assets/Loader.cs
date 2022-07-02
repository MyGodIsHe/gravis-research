using System.Collections.Generic;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class Loader : MonoBehaviour
{
    public string filePath;

    public void Load()
    {
        var stream = CharStreams.fromPath(filePath);
        var lexer = new GravisLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new GravisParser(tokens)
        {
            BuildParseTree = true
        };
        var loader = new LoadListener();
        var tree = parser.file_input();
        ParseTreeWalker.Default.Walk(loader, tree);

        RelinkSubspaces(loader.subspaces, loader.nodes);

        GraphManager.Get().Init(loader.nodes);
    }


    private static void RelinkSubspaces(List<Subspace> subspaces, List<Node> nodes)
    {
        foreach (var subspace in subspaces)
        {
            foreach (var input_node in subspace.node.inputs)
            {
                input_node.outputs.Clear();
                input_node.outputs.Add(subspace.input);
                subspace.input.inputs.Add(input_node);
                subspace.input.inputs.Remove(subspace.node);
            }
            foreach (var output_node in subspace.node.outputs)
            {
                output_node.inputs.Clear();
                output_node.inputs.Add(subspace.output);
                subspace.output.outputs.Add(output_node);
                subspace.output.outputs.Remove(subspace.node);
            }
            nodes.Remove(subspace.node);
        }
    }
}
